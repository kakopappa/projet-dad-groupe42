using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SessionService
{
    [ServiceBehavior(Name = "Service", InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service : IServiceClient, IServiceWorkflow, IServiceFournisseur, IServiceAdministrator
    {
        #region CLIENT SERVICE

        public bool DisconnectClient(Guid sessionID)
        {
            return this.Disconnect(sessionID, UserType.CLIENT);
        }

        public bool ActivateClient(Guid sessionID, Guid userID)
        {
            return this.Activate(sessionID, userID);
        }

        public ItemState AddItemInCart(Guid sessionID, Guid itemID, int quantity)
        {
            ItemState state = ItemState.NOT_VERIFIED;
            Session session = (from Session s in Sessions.Instance
                               where s.SessionID == sessionID && s.Active
                               select s).FirstOrDefault<Session>();

            if (session != null)
            {
                session.LastActivity = DateTime.Now;
                CartEntrie item = (from CartEntrie c in session.CartEntries
                                   where c.ObjectID == itemID
                                   select c).FirstOrDefault<CartEntrie>();
                if (item != null)
                {
                    quantity += item.Quantity;
                }
                if (quantity > 0)
                {
                    try
                    {
                        AddInCartVerificationItem.ServiceClient client = new AddInCartVerificationItem.ServiceClient();
                        state = (ItemState)client.GetItemVerification(itemID, quantity);
                        if (state == ItemState.OK)
                        {
                            if (item != null)
                                item.Quantity = quantity;
                            else
                                session.CartEntries.Add(new CartEntrie(itemID, quantity));
                        }
                        else if (item != null && (state == ItemState.UNAVAILABLE || state == ItemState.UNKNOW))
                        {
                            session.CartEntries.Remove(item);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            return state;
        }

        public bool RemoveItemInCart(Guid sessionID, Guid itemID)
        {
            Session session = (from Session s in Sessions.Instance
                               where s.SessionID == sessionID && s.Active
                               select s).FirstOrDefault<Session>();

            if (session != null)
            {
                session.LastActivity = DateTime.Now;
                CartEntrie item = (from CartEntrie c in session.CartEntries
                                   where c.ObjectID == itemID
                                   select c).FirstOrDefault<CartEntrie>();
                if (item != null)
                {
                    session.CartEntries.Remove(item);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveCartContentClient(Guid sessionID)
        {
            return this.RemoveCartContent(sessionID);
        }

        public bool ChatUpdateClient(Guid sessionID, Guid correspondentID, ChatState state, string message)
        {
            return this.ChatUpdate(sessionID, correspondentID, state, message);
        }

        #endregion

        #region FOURNISSEUR SERVICE

        public bool DisconnectFournisseur(Guid sessionID)
        {
            return this.Disconnect(sessionID, UserType.FOURNISSEUR);
        }

        public bool ActivateFournisseur(Guid sessionID, Guid userID)
        {
            return this.Activate(sessionID, userID);
        }

        public bool ChatUpdateFournisseur(Guid sessionID, Guid correspondentID, ChatState state, string message)
        {
            return this.ChatUpdate(sessionID, correspondentID, state, message);
        }

        #endregion

        #region ADMINISTRATOR SERVICE

        public Guid Connect()
        {
            var qry = (from Session s in Sessions.Instance
                       where s.Type == UserType.ADMINISTRATOR
                       select s).FirstOrDefault<Session>();

            if (qry != null)
            {
                Sessions.RemoveSession(qry);
            }

            Session session = new Session(Guid.Empty, UserType.ADMINISTRATOR);
            session.User = OperationContext.Current.GetCallbackChannel<IAdministrator>();
            session.LastActivity = DateTime.Now;
            session.Active = true;
            Sessions.Instance.Add(session);
            return session.SessionID;
        }

        public bool ChatUpdateAdministrator(Guid sessionID, Guid correspondentID, ChatState state, string message)
        {
            return this.ChatUpdate(sessionID, correspondentID, state, message);
        }

        public bool DisconnectAdministrateur(Guid sessionID)
        {
            return this.Disconnect(sessionID, UserType.ADMINISTRATOR);
        }

        #endregion

        #region WORKFLOW SERVICE

        public void AdminProductModification(Guid itemID, Guid fournisseurID, bool deleted)
        {
            var session = (from Session s in Sessions.Instance
                       where s.UserID == fournisseurID && s.Type == UserType.FOURNISSEUR && s.Active
                       select s).FirstOrDefault<Session>();

            if (session != null)
            {
                try
                {
                    ((IFournisseur)session.User).ProductNotification(itemID, deleted);
                }
                catch { }
            }
        }

        public Guid CreateSession(Guid userID, UserType type)
        {
            return Sessions.AddSession(userID, type);
        }

        public bool RemoveCartContent(Guid sessionID)
        {
            var session = (from Session s in Sessions.Instance
                           where s.SessionID == sessionID && s.CartEntries.Count > 0
                           select s).FirstOrDefault<Session>();
            if (session != null)
            {
                session.LastActivity = DateTime.Now;
                session.CartEntries.Clear();
                return true;
            }
            return false;
        }

        public Guid GetUserID(Guid sessionID)
        {
            Session session = (from Session s in Sessions.Instance
                               where s.SessionID == sessionID
                               select s).FirstOrDefault<Session>();

            if (session != null)
            {
                session.LastActivity = DateTime.Now;
                return session.UserID;
            }
            return Guid.Empty;
        }

        public void ChangeItemAvailability(Guid itemID, int quantity, bool available, bool exist)
        {
            var qry = from Session s in Sessions.Instance
                      from CartEntrie c in s.CartEntries
                      where c.ObjectID == itemID
                      select new { session = s, item = c };

            foreach (var o in qry)
            {
                try
                {
                    if (!exist)
                    {
                        o.session.CartEntries.Remove(o.item);
                        ((IClient)o.session.User).CartNotification(itemID, 0, ItemState.UNKNOW);
                    }
                    else if(!available)
                    {
                        o.session.CartEntries.Remove(o.item);
                        ((IClient)o.session.User).CartNotification(itemID, 0, ItemState.UNAVAILABLE);
                    }
                    else if (quantity == 0)
                    {
                        o.session.CartEntries.Remove(o.item);
                        ((IClient)o.session.User).CartNotification(itemID, 0, ItemState.INSUFFICIENT);
                    }
                    else if (quantity < o.item.Quantity)
                    {
                        o.item.Quantity = quantity;
                        ((IClient)o.session.User).CartNotification(itemID, quantity, ItemState.OK);
                    }
                    o.session.LastActivity = DateTime.Now;
                }
                catch
                {
                }
            }
        }

        public UserType GetSessionType(Guid sessionID)
        {
            Session session = (from Session s in Sessions.Instance
                               where s.SessionID == sessionID && s.Active
                               select s).FirstOrDefault<Session>();

            if (session != null)
            {
                session.LastActivity = DateTime.Now;
                return session.Type;
            }
            return UserType.UNKNOW;
        }

        public CartEntrie[] GetCart(Guid sessionID)
        {
            var qry = (from Session s in Sessions.Instance
                       where s.SessionID == sessionID
                       select s.CartEntries).FirstOrDefault();
            if (qry != null)
            {
                return qry.ToArray<CartEntrie>();
            }
            return null;             
        }

        #endregion

        private bool ChatUpdate(Guid sessionID, Guid correspondentID, ChatState state, string message)
        {
            var user = (from Session s in Sessions.Instance
                        where s.SessionID == sessionID && s.Active
                        select s).FirstOrDefault<Session>();

            var correspondent = (from Session s in Sessions.Instance
                                 where s.UserID == correspondentID
                                 select s).FirstOrDefault<Session>();

            if (user != null && correspondent != null)
            {
                if (!user.InChatWith.Contains(correspondent.SessionID) && state == ChatState.MESSAGED)
                    user.InChatWith.Add(correspondent.SessionID);
                try
                {
                    switch (user.Type)
                    {
                        case UserType.ADMINISTRATOR:
                            ((IAdministrator)correspondent.User).ChatNotificationAdministrator(user.UserID, string.Empty, ChatState.DISCONNECTED);
                            break;
                        case UserType.CLIENT:
                            ((IClient)correspondent.User).ChatNotificationClient(user.UserID, string.Empty, ChatState.DISCONNECTED);
                            break;
                        case UserType.FOURNISSEUR:
                            ((IFournisseur)correspondent.User).ChatNotificationFournisseur(user.UserID, string.Empty, ChatState.DISCONNECTED);
                            break;
                    }
                }
                catch
                {
                    return false;
                }
                if (state != ChatState.MESSAGED)
                    user.InChatWith.Remove(correspondent.SessionID);
                return true;
            }
            return false;
        }

        private bool Activate(Guid sessionID, Guid userID)
        {
            Session session = (from Session s in Sessions.Instance
                               where s.SessionID == sessionID && s.UserID == userID && !s.Active
                               select s).FirstOrDefault<Session>();
            if (session != null)
            {
                session.Active = true;
                session.LastActivity = DateTime.Now;
                switch (session.Type)
                {
                    case UserType.CLIENT : 
                        session.User = OperationContext.Current.GetCallbackChannel<IClient>();
                        break;
                    case UserType.FOURNISSEUR :
                        session.User = OperationContext.Current.GetCallbackChannel<IFournisseur>();
                        break;
                }
                return true;
            }
            else
                return false;
        }

        private bool Disconnect(Guid sessionID, UserType type)
        {
            var session = (from Session s in Sessions.Instance
                           where s.SessionID == sessionID && s.Type == type
                           select s).FirstOrDefault<Session>();

            if (session != null)
            {
                Sessions.RemoveSession(session);
                return true;
            }
            return false;
        }
    }
}

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
        public bool ActivateClient(Guid sessionID, Guid userID)
        {
            return this.Activate(sessionID, userID);
        }

        public bool ActivateFournisseur(Guid sessionID, Guid userID)
        {
            return this.Activate(sessionID, userID);
        }

        private bool Activate(Guid sessionID, Guid userID)
        {
            Session session = (from Session s in Sessions.Instance
                               where s.SessionID == sessionID && s.UserID == userID && s.Active == false
                               select s).FirstOrDefault<Session>();
            if (session != null)
            {
                session.Active = true;
                session.LastActivity = DateTime.Now;
                session.User = OperationContext.Current.GetCallbackChannel<IClient>();
                return true;
            }
            else
                return false;
        }

        public ItemState AddItemInCart(Guid sessionID, Guid itemID, int quantity)
        {
            ItemState state = ItemState.NOT_VERIFIED;
            Session session = (from Session s in Sessions.Instance
                               where s.SessionID == sessionID
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
                               where s.SessionID == sessionID
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

        public Guid CreateSession(Guid userID, UserType type)
        {
            return Sessions.AddSession(userID, type);
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

        public bool RemoveCartContentClient(Guid sessionID)
        {
            return this.RemoveCartContent(sessionID);
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
            Sessions.Instance.Add(session);
            return session.SessionID;
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

        public bool ChatUpdateClient(Guid sessionID, Guid correspondentID, ChatState state, string message)
        {
            return this.ChatUpdate(sessionID, correspondentID, state, message);
        }

        public bool ChatUpdateFournisseur(Guid sessionID, Guid correspondentID, ChatState state, string message)
        {
            return this.ChatUpdate(sessionID, correspondentID, state, message);
        }

        public bool ChatUpdateAdministrator(Guid sessionID, Guid correspondentID, ChatState state, string message)
        {
            return this.ChatUpdate(sessionID, correspondentID, state, message);
        }

        private bool ChatUpdate(Guid sessionID, Guid correspondentID, ChatState state, string message)
        {
            var user = (from Session s in Sessions.Instance
                        where s.SessionID == sessionID
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
                    ((IUser)correspondent.User).ChatNotification(user.UserID, message, state);
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

        public UserType GetSessionType(Guid sessionID)
        {
            Session session = (from Session s in Sessions.Instance
                               where s.SessionID == sessionID
                               select s).FirstOrDefault<Session>();

            if (session != null)
            {
                session.LastActivity = DateTime.Now;
                return session.Type;
            }
            return UserType.UNKNOW;
        }
    }
}

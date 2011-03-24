using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SessionService
{
    public class Sessions : List<Session>
    {
        private static volatile Sessions instance;
        private static object syncRoot = new Object(); 

        public static Sessions Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Sessions();
                    }
                }

                return instance;
            }
        }

        private Sessions()
            : base()
        { }

        public static Guid AddSession(Guid guid, UserType type)
        {
            Session currentSession = (from Session s in Instance
                                 where s.UserID == guid
                                 select s).FirstOrDefault<Session>();
            if (currentSession != null)
                RemoveSession(currentSession);
            Instance.Add(new Session(guid, type));
            return (from Session s in Instance
                    where s.UserID == guid
                    select s).FirstOrDefault<Session>().SessionID;
        }

        public static void RemoveSession(Session session)
        {
            try
            {
                switch (session.Type)
                {
                    case UserType.ADMINISTRATOR:
                        ((IAdministrator)session.User).DisconnectedAdministrator();
                        break;
                    case UserType.CLIENT:
                        ((IClient)session.User).DisconnectedClient();
                        break;
                    case UserType.FOURNISSEUR:
                        ((IFournisseur)session.User).DisconnectedFournisseur();
                        break;
                }
            }
            catch
            {
            }
            foreach (Guid sessionID in session.InChatWith)
            {
                try
                {
                    var qry = (from Session s in Instance
                                  where s.SessionID == sessionID
                                  select s).FirstOrDefault<Session>();
                    qry.InChatWith.Remove(session.SessionID);
                    switch (qry.Type)
                    {
                        case UserType.ADMINISTRATOR:
                            ((IAdministrator)qry.User).ChatNotificationAdministrator(session.UserID, string.Empty, ChatState.DISCONNECTED);
                            break;
                        case UserType.CLIENT:
                            ((IClient)qry.User).ChatNotificationClient(session.UserID, string.Empty, ChatState.DISCONNECTED);
                            break;
                        case UserType.FOURNISSEUR:
                            ((IFournisseur)qry.User).ChatNotificationFournisseur(session.UserID, string.Empty, ChatState.DISCONNECTED);
                            break;
                    }
                }
                catch
                {
                }
            }
            Instance.Remove(session);
        }
    }
}
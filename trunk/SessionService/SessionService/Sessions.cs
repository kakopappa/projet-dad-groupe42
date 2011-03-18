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
            switch (session.Type)
            {
                case UserType.CLIENT :
                    try
                    {
                        ((IClient)session.User).Disconnected();
                    }
                    catch
                    {
                    }
                    break;
            }
            Instance.Remove(session);
        }
    }
}
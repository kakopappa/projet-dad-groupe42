using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SessionService
{
    public class Session
    {
        private List<CartEntrie> cartEntries;
        private List<Guid> inChatWith;
        private Guid sessionID;
        private Guid userID;
        private UserType type;
        private DateTime lastActivity;
        private DateTime creation;
        private Object user;
        private bool active;

        public Guid SessionID
        {
            get
            {
                return this.sessionID;
            }
        }

        public Guid UserID
        {
            get
            {
                return this.userID;
            }
        }

        public List<CartEntrie> CartEntries
        {
            get
            {
                return this.cartEntries;
            }
        }

        public DateTime LastActivity
        {
            get
            {
                return this.lastActivity;
            }
            set
            {
                this.lastActivity = value;
            }
        }

        public DateTime Creation
        {
            get
            {
                return this.creation;
            }
        }

        public UserType Type
        {
            get
            {
                return this.type;
            }
        }

        public Object User
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = value;
            }
        }

        public bool Active
        {
            get
            {
                return this.active;
            }
            set
            {
                this.active = value;
            }
        }

        public List<Guid> InChatWith
        {
            get
            {
                return this.inChatWith;
            }
        }

        public Session(Guid userID, UserType type)
        {
            this.userID = userID;
            this.type = type;
            this.sessionID = Guid.NewGuid();
            this.creation = DateTime.Now;
            this.lastActivity = DateTime.MinValue;
            this.active = false;
            this.cartEntries = new List<CartEntrie>();
            this.inChatWith = new List<Guid>();
        }
    }
}
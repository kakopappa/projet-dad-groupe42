using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SessionService
{
    [DataContract]
    public class CartEntrie
    {
        private Guid guid;
        private int quantity;

        [DataMember]
        public Guid ObjectID
        {
            set
            {
                this.guid = value;
            }
            get
            {
                return this.guid;
            }
        }
        [DataMember]
        public int Quantity
        {
            get
            {
                return this.quantity;
            }
            set
            {
                this.quantity = value;
            }
        }

        public CartEntrie(Guid guid, int quantities)
        {
            this.quantity = quantities;
            this.guid = guid;
        }
    }
}
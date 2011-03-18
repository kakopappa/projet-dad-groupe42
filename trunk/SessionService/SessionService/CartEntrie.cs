using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SessionService
{
    public class CartEntrie
    {
        private Guid guid;
        private int quantity;

        public Guid ObjectID
        {
            get
            {
                return this.guid;
            }
        }

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
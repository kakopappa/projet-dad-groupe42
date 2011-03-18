using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Events
{
    public delegate void ObjectEventHandler(object sender, ObjectEventArgs e);

    public class ObjectEventArgs : EventArgs
    {
        public Object Data { get; set; }

        public ObjectEventArgs(Object obj)
        {
            this.Data = obj;
        }
    }
}

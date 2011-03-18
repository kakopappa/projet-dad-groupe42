using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Events
{
    public class UIEventArgs : EventArgs
    {
        public Enum Response { get; set; }

        public UIEventArgs(Enum resp)
        {
            this.Response = resp;
        }
    }
}

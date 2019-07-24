using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo15
{
   public class Proxy
    {
        public Proxy()
        {
        }
        public Proxy(string iP, int port, int state)
        {
            IP = iP;
            Port = port;
            State = state;
        }

        public string IP { get; set; }
        public int Port { get; set; }
        public int State { get; set; }
    }
}

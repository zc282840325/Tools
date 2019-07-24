using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo15
{
   public class Model
    {
        public Model()
        {
        }

        public Model(string url, string xpath, string title, State state)
        {
            this.url = url;
            this.xpath = xpath;
            this.title = title;
            this.state = state;
        }
        public string url { get; set; }
        public string xpath { get; set; }
        public string title { get; set; }
        public State state { get; set; }

        
    }

    public enum State
    {
        Quantity,//词条数量
        Entry,//参考资料数量
        Fan//粉丝数量
    }
}

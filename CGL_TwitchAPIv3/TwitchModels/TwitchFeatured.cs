using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGL.TwitchModels
{
    public class TwitchFeatured
    {
        public string text { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public bool? sponsored { get; set; }
        // NOTE(duan): ... really
        public System.Int32 priority { get; set; }
        public bool? scheduled { get; set; }
        public TwitchStream stream { get; set; }
    }
}

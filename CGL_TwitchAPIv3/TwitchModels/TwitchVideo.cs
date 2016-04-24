using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGL.TwitchModels
{
    public class TwitchVideo
    {
        public string title { get; set; }
        public string description { get; set; }
        public System.Int64 broadcast_id { get; set; }
        public string broadcast_type { get; set; }
        public string status { get; set; }
        public string tag_list { get; set; }

        // NOTE(duan): probably need a bigger int than this for large channels
        public int views { get; set; }

        public string created_at { get; set; }

        // NOTE(duan): reponse has a letter in it, wierd
        //v59563248
        public string _id { get; set; }

        public string recorded_at { get; set; }
        public string game { get; set; }
        public int length { get; set; }
        public string preview { get; set; }

        public List<TwitchThumbnail> thumbnails { get; set; }

        public string url { get; set; }

        public Dictionary<string, double> fps { get; set; }

        public Dictionary<string, string> resolutions { get; set; }
        public Dictionary<string, string> _links { get; set; }
        public Dictionary<string, string> channel { get; set; }
    }

    public class TwitchThumbnail
    {
        public string url { get; set; }
        public string type { get; set; }
    }

    public class TwitchVideo_Response : TwitchVideo
    {
    }

    public class TwitchVideoList_Response
    {
        public Dictionary<string, string> _links { get; set; }
        public List<TwitchVideo> videos { get; set; }

        // NOTE(duan): why is this here...
        // "_total": 179,
        public int _total {get; set;}
          
    }
}

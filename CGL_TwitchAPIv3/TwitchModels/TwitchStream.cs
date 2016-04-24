using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGL.TwitchModels
{
    public class TwitchStream
    {
        public System.Int64 _id { get; set; }
        public string game { get; set; }
        public int viewers { get; set; }
        public string created_at { get; set; }
        public int video_height { get; set; }
        public float average_fps { get; set; }
        public int delay { get; set; }
        public bool is_playlist { get; set; }
        public Dictionary<string, string> _links { get; set; }
        public Dictionary<string, string> preview { get; set; }
        public TwitchChannel channel { get; set; }
    }

    /// <summary>
    /// Alias for TwitchStream to match other Response Types.
    /// </summary>
    public class TwitchStream_Repsonse
    {
        // NOTE(duan): TwitchAPI so ghetto
        public TwitchStream stream { get; set; }
        public Dictionary<string, string> _links { get; set; }
    }

    public class TwitchStreamList_Response
    {
        public List<TwitchStream> streams { get; set; }
        public int _total { get; set; }
        public Dictionary<string, string> _links { get; set; }
    }

    
    public class TwitchStreamFeaturedList_Response
    {
        public List<TwitchFeatured> featured { get; set; }
        public Dictionary<string, string> _links { get; set; }
    }

    /// <summary>
    /// Alias for TwitchStreamList_Response to match Response Types
    /// </summary>
    public class TwitchSearchStreamList_Response : TwitchStreamList_Response
    {
    }

    public class TwitchStreamSummary
    {
        public System.Int64 viewers { get; set; }
        public Dictionary<string, string> _links { get; set; }
        public System.Int64 channels { get; set; }

        public override string ToString()
        {
            return string.Format("#Channels:{0} #Viewers:{1}", channels, viewers);
        }
    }

    /// <summary>
    /// Alias for TwitchStreamSummary to match other Reponse Types.
    /// </summary>
    public class TwitchStreamSummary_Response : TwitchStreamSummary
    {
    }

}

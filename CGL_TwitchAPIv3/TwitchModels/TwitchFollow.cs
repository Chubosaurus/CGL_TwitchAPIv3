using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGL.TwitchModels
{
    /// <summary>
    /// A TwitchFollow
    /// </summary>
    public class TwitchChannelFollow
    {
        public string created_at { get; set; }
        public Dictionary<string, string> _links { get; set; }
        public bool notifications { get; set; }
        public TwitchChannel channel { get; set; }
    }

    /// <summary>
    /// A Response to a query of Twitch Channel Follows
    /// </summary>
    public class TwitchChannelFollowList_Response
    {
        public Dictionary<string, string> _links { get; set; }
        public System.UInt64 _total { get; set; }
        public List<TwitchChannelFollow> follows { get; set; }
    }

    /// <summary>
    /// Alias for TwitchChannelFollow to match other Response Type
    /// </summary>
    public class TwitchUserChannelRelationship_Response : TwitchChannelFollow
    {
    }

    /// <summary>
    /// Alias for when an TwitchUser follows a channel.
    /// </summary>
    public class TwitchChannelFollowed_Response : TwitchChannelFollow
    {
    }
}

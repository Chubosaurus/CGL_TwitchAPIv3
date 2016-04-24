using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CGL.TwitchModels
{
    /// <summary>
    /// A Twitch User.
    /// </summary>
    public class TwitchUser
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Created_At { get; set; }
        public string Updated_At { get; set; }
        public Dictionary<string, string> _links { get; set; }
        public string Logo { get; set; }
        public int _id { get; set; }
        public string Display_name { get; set; }
        public string Bio { get; set; }
    }

    /// <summary>
    /// A Authenticated Twitch User (basically a TwitchUser with more properties).
    public class TwitchAuthenticatedUser : TwitchUser
    /// </summary>
    {
        public string Email { get; set; }
        public bool Partnered { get; set; }
        public Dictionary<string, bool> Notifications { get; set; }
    }

    /// <summary>
    /// An alias for the TwitchUser class to match other reponse types.
    /// </summary>
    public class TwitchUser_Response : TwitchUser
    {
    }

    /// <summary>
    /// A block user.
    /// </summary>
    public class TwitchBlockedUser
    {
        public int _id { get; set; }
        public Dictionary<string, string> _links { get; set; }
        public TwitchUser User { get; set; }        
    }

    /// <summary>
    /// An alias for the TwitchBlockUser class to match other reponse types.
    /// </summary>
    public class TwitchBlockedUser_Response : TwitchBlockedUser
    {
    }

    /// <summary>
    /// Response from a query to get a list of blocked users.
    /// </summary>
    public class TwitchBlockUserList_Response
    {
        public Dictionary<string, string> _links { get; set; }
        public List<TwitchBlockedUser> Blocks { get; set; }
    }

    /// <summary>
    /// Response from a query to get a channel's list of editors.
    /// </summary>
    public class TwitchEditorList_Response
    {
        public Dictionary<string, string> _links { get; set; }                
        public List<TwitchUser> users {get; set;}
    }

    public class TwitchFollowUser
    {
        public string created_at { get; set; }
        public Dictionary<string, string> _links { get; set; }
        public bool notifications { get; set; }
        public TwitchUser user { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TwitchFollowList_Response
    {
        public int _total { get; set; }
        public Dictionary<string, string> _links { get; set; }
        public string _cursor { get; set; }
        public List<TwitchFollowUser> follows { get; set; }
    }
}
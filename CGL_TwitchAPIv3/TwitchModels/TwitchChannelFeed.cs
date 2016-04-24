using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CGL.TwitchModels
{
    /// <summary>
    /// A Twitch Post.
    /// </summary>
    public class TwitchPost
    {
        public System.UInt64 Id { get; set; }
        public string Created_At { get; set; }
        public bool Deleted { get; set; }
        public List<TwitchPostEmotes> Emotes { get; set; }
        public TwitchPostReaction Reactions { get; set; }
        public string Body { get; set; }
        public TwitchUser User { get; set; }

        #region [Internal DataStructures -------------------------------------]

        public class TwitchPostEmotes
        {
            public int Id { get; set; }
            public int Start { get; set; }
            public int End { get; set; }
        }

        public class TwitchPostReaction
        {
            public class TwitchPostReactionEndorse
            {
                public string Emote { get; set; }
                public int Count { get; set; }
                public List<int> User_Ids { get; set; }
            }

            public TwitchPostReactionEndorse Endorse { get; set; }
        }

        #endregion
    }

    /// <summary>
    /// Alias for a TwitchPost to match other response type.
    /// </summary>
    public class TwitchPost_Response : TwitchPost
    {
        [JsonIgnore]
        public bool IsDeleted { get { return Deleted; } }
    }

    /// <summary>
    /// A Twitch Post with a Twitter link.
    /// Twitch API will respond with this when posting to the API.
    /// </summary>
    public class TwitchPostTweet_Response
    {
        public TwitchPost Post { get; set; }
        public string Tweet { get; set; }
    }

    /// <summary>
    /// A List of TwitchPost(s).
    /// </summary>
    public class TwitchPostList_Response
    {
        public int _Total { get; set; }
        public string _Cursor { get; set; }
        public List<TwitchPost> Posts { get; set; }
    }

    /// <summary>
    /// A Response to a TwitchPostReaction (Posting a Reaction to a Post).
    /// </summary>
    public class TwitchPostReaction_Response
    {
        public System.UInt64 Id { get; set; }
        public string Created_At { get; set; }
        public string Emote_Id { get; set; }
        public TwitchUser User { get; set; }
    }

    /// <summary>
    /// A Response to when deleting a TwitchReaction.
    /// </summary>
    public class TwitchDeleteReaction_Response
    {
        public bool Deleted { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CGL.TwitchModels
{
    /// <summary>
    /// Twitch Channel object.
    /// </summary>
    public class TwitchChannel
    {
        // NOTE(duan): some channels have mature not set, so we need a nullable bool
        public bool? mature { get; set; }
        public string status { get; set; }
        public string broadcaster_language { get; set; }
        public string display_name { get; set; }
        public string game { get; set; }
        public int? delay { get; set; }
        public string language { get; set; }

        // NOTE(duan): might need a bigger int
        public int _id { get; set; }

        public string name { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string logo { get; set; }
        public string banner { get; set; }
        public string video_banner { get; set; }
        public string background { get; set; }
        public string profile_banner { get; set; }

        // NOTE(duan): figure out if this is a hexcode or just a plain string
        public string profile_banner_background_color { get; set; }

        public bool partner { get; set; }
        public string url { get; set; }
        public int views { get; set; }
        public int followers { get; set; }

        public Dictionary<string, string> _links { get; set; }

        #region [Authenticated Properties ------------------------------------]
        public string email { get; set; }
        public string stream_key { get; set; }
        #endregion
    }

    /// <summary>
    /// Alias for TwitchChannel to match other response type.
    /// </summary>
    public class TwitchChannel_Response : TwitchChannel
    {
    }

    public class TwitchChannelSub_Response
    {
        public Dictionary<string, string> _links { get; set; }
        public TwitchChannel channel { get; set; }
        public string _id { get; set; }
        public string created_at { get; set; }
    }

    /// <summary>
    /// Twitch Channel Properties.
    /// This is what gets posted to change/update the Channel's Properties.
    /// </summary>
    public class TwitchChannelProperties
    {
        #region Internal Data Class

        public class TwitchChannelPropertiesData
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string status { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string game { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string delay { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool? channel_feed_enabled { get; set; }
        }

        #endregion

        public TwitchChannelProperties()
        {
            this.channel = new TwitchChannelPropertiesData();
        }

        public TwitchChannelPropertiesData channel { get; set; }

        #region [Quick Access ------------------------------------------------]

        [JsonIgnore]
        public string Status
        {
            get { return channel.status; }
            set { channel.status = value; }
        }

        [JsonIgnore]
        public string Game
        {
            get { return channel.game; }
            set { channel.game = value; }
        }

        [JsonIgnore]
        public string Delay
        {
            get { return channel.delay; }
            set { channel.delay = value; }
        }

        [JsonIgnore]
        public bool? ChannelFeedEnabled
        {
            get { return channel.channel_feed_enabled; }
            set { channel.channel_feed_enabled = value; }
        }

        #endregion
    }

    /// <summary>
    /// A Response to a Search Query.
    /// </summary>
    public class TwitchSearchChannelsList_Response
    {
        public int _total { get; set; }
        public Dictionary<string, string> _links { get; set; }
        public List<TwitchChannel> channels { get; set; }
    }
}

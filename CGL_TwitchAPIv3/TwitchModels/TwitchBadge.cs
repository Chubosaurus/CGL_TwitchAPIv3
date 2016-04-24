using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace CGL.TwitchModels
{
    /// <summary>
    /// A TwitchBadge is the image associated with TwitchUser when chatting.
    /// This image should display before the TwitchUser's DisplayName to let others know
    /// of their status (for example, subscriber/mod/staff/etc).
    /// </summary>
    public class TwitchBadge
    {
        public string Alpha { get; set; }
        public string Image { get; set; }
        public string Svg { get; set; }
    }

    /// <summary>
    /// A Response to GET of a Channel's Badges.
    /// </summary>
    public class TwitchBadge_Response
    {
        public TwitchBadge_Response()
        {
            this._badges = new Dictionary<string, TwitchBadge>();
        }

        public Dictionary<string, string> _links { get; set; }
        public TwitchBadge Admin { get; set; }
        public TwitchBadge Broadcaster { get; set; }
        public TwitchBadge Global_mod { get; set; }
        public TwitchBadge Mod { get; set; }
        public TwitchBadge Staff { get; set; }
        public TwitchBadge Subscriber { get; set; }
        public TwitchBadge Turbo { get; set; }

        /// <summary>
        /// Setup easy access via a dictionary.
        /// </summary>
        public void SetupDictionary()
        {
            if (_badges != null)
            {
                _badges["admin"] = this.Admin;
                _badges["broadcaster"] = this.Broadcaster;
                _badges["global_mod"] = this.Global_mod;
                _badges["mod"] = this.Mod;
                _badges["staff"] = this.Staff;
                _badges["subscriber"] = this.Subscriber;
                _badges["turbo"] = this.Turbo;
            }
        }

        [JsonIgnore]
        private Dictionary<string, TwitchBadge> _badges;

        [JsonIgnore]
        public Dictionary<string, TwitchBadge> Badges
        {
            get { return _badges; }
            protected set
            {
                if (_badges != value)
                {
                    _badges = value;
                }
            }
        }
    }
}

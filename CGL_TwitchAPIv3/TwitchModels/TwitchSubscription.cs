using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGL.TwitchModels
{
    public class TwitchSubscription
    {
        public string _id { get; set; }
        public TwitchUser user { get; set; }
    }

    /// <summary>
    /// Alias for TwitchSubscription to match other Response Types.
    /// </summary>
    public class TwitchSubscription_Response : TwitchSubscription
    {
        public string created_at { get; set; }
        public Dictionary<string, string> _links { get; set; }
    }

    public class TwitchSubscriptionList_Response
    {
        public int _total { get; set; }
        public Dictionary<string, string> _links { get; set; }
        public List<TwitchSubscription> subscriptions { get; set; }
    }
}

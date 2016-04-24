using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CGL.TwitchModels
{
    /// <summary>
    /// A TwitchIngest.
    /// </summary>
    public class TwitchIngest
    {
        public string name { get; set; }
       
        [JsonProperty("default")]
        // NOTE(duan): "default" is a C# keyword
        public bool Default { get; set; }

        public int id { get; set; }
        public string url_template { get; set; }
        public double availability { get; set; }
    }

    /// <summary>
    /// A List of TwitchIngests.
    /// </summary>
    public class TwitchIngestsList_Response
    {
        public Dictionary<string, string> _links { get; set; }
        public List<TwitchIngest> ingests { get; set; }
    }
}

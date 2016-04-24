using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGL.TwitchModels
{
    /// <summary>
    /// TwitchLinks (basically a list of URLS by Id).
    /// </summary>
    public class TwitchLinks
    {
        public Dictionary<string, string> _links { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGL.TwitchModels
{
    public class TwitchTeam
    {
        public string info { get; set; }
        public Dictionary<string, string> _links { get; set; }
        public string background { get; set; }
        public string banner { get; set; }
        public string name { get; set; }
        public int _id { get; set; }
        public string updated_at { get; set; }
        public string display_name { get; set; }
        public string created_at { get; set; }
        public string logo { get; set; }
    }

    public class TwitchTeam_Response : TwitchTeam
    {
    }

    public class TwitchTeamList_Response
    {
        public Dictionary<string, string> _links { get; set; }
        public List<TwitchTeam> teams { get; set; }
    }
}

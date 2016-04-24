using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CGL.TwitchModels
{
    /// <summary>
    /// A TwitchGame.
    /// </summary>
    public class TwitchGame
    {
        public System.Int64 viewers { get; set; }
        public int channels { get; set; }
        public TwitchGameData game { get; set; }

        public override string ToString()
        {
            return string.Format("{0} #Viewers:{1} #Channels:{2}", game.name, viewers, channels);
        }
    }

    /// <summary>
    /// Main data for a TWitchGame.
    /// </summary>
    public class TwitchGameData
    {
        public string name { get; set; }
        public Dictionary<string, string> box { get; set; }
        public Dictionary<string, string> logo { get; set; }
        public Dictionary<string, string> _links { get; set; }
        public int _id { get; set; }
        public int giantbomb_id { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", name);
        }
    }

    /// <summary>
    /// A Response to a query for a List of TwitchGames.
    /// </summary>
    public class TwitchGamesList_Response
    {
        public Dictionary<string, string> _links { get; set; }
        public int _total { get; set; }
        public List<TwitchGame> top { get; set; }
        
        [JsonIgnore]
        public int TotalNumberOfGames
        {
            get { return _total; }
        }

        [JsonIgnore]
        public int NumberOfGames
        {
            get { return top.Count; }
        }

        public override string ToString()
        {
            return string.Format("Count:{0} Total:{1}", NumberOfGames, _total);
        }
    }


    public class TwitchSearchGameData : TwitchGameData
    {
        public string name { get; set; }
        public Dictionary<string, string> box { get; set; }
        public Dictionary<string, string> logo { get; set; }
        public Dictionary<string, string> _links { get; set; }
        public int _id { get; set; }
        public int giantbomb_id { get; set; }
        public int popularity { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", name);
        }
    }

    public class TwitchSearchGamesList_Response
    {
        public Dictionary<string, string> _links { get; set; }
        public List<TwitchSearchGameData> games { get; set; }
    }
}

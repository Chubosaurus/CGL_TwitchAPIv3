using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGL.TwitchModels
{
    /// <summary>
    /// A TwitchEmoticon contains the TwitchImage and a regular expression to match.
    /// </summary>
    public class TwitchEmoticon
    {
        public string regex { get; set; }
        public List<TwitchImage> Images { get; set; }
    }

    /// <summary>
    /// The response to getting the entire list of TwitchEmoticons.
    /// </summary>
    public class TwitchEmoticonList_Response
    {
        public Dictionary<string, string> _links { get; set; }
        public List<TwitchEmoticon> Emoticons { get; set; }
    }

    /// <summary>
    /// The chat data to describe the TwitchEmoticon.
    /// </summary>
    public class TwitchEmoticonData
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? Emoticon_Set { get; set; }
    }

    /// <summary>
    /// The response to getting the entire list of TwitchImages used in emoticons.
    /// </summary>
    public class TwitchEmoticonImageList_Response
    {
        public List<TwitchEmoticonData> Emoticons;
    }

    /// <summary>
    /// The data to describe the Emoticon in a set.
    /// </summary>
    public class TwitchEmoticonSetData
    {
        public int Id { get; set; }
        public string Code { get; set; }
    }

    /// <summary>
    /// The response to getting the list of TwitchEmoticon in specific list of sets.
    /// </summary>
    public class TwitchEmoticonSet_Response
    {
        public Dictionary<string, List<TwitchEmoticonSetData>> Emoticon_Sets;
    }
}

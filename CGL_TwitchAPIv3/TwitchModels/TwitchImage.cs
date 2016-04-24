using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGL.TwitchModels
{
    /// <summary>
    /// A TwitchImage describes an image used as an Emoticon.
    /// </summary>
    public class TwitchImage
    {
        public string emoticon_set { get; set; }                // the emoticon set the image belongs to
        public int? height { get; set; }                        // the height of the image if any
        public int? width { get; set; }                         // the width of the image if any
        public string url { get; set; }                         // the absolute url of the image
    }
}

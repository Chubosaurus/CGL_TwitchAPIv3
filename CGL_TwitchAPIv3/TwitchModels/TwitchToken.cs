using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGL.TwitchModels
{
    /// <summary>
    /// TwitchToken containing the TwitchAutorization and validity.
    /// </summary>
    public class TwitchToken
    {
        public TwitchAuthorization Authorization { get; set; }
        public string User_Name { get; set; }
        public bool Valid { get; set; }
    }

    /// <summary>
    /// TwitchAuthorization containing the scope(s) and timestamps.
    /// </summary>
    public class TwitchAuthorization
    {
        public List<string> scopes { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        /// <summary>
        /// Get the TwitchScope enum from the list of scopes.
        /// </summary>
        public TwitchScope TwitchScope
        {
            get
            {
                if (scopes == null)
                {
                    return 0;
                }
                else
                {
                    TwitchScope s = 0;

                    foreach (string scope in scopes)
                    {
                        foreach (TwitchScope ts in Enum.GetValues(typeof(TwitchScope)))
                        {
                            if (ts.ToString() == scope)
                            {
                                s = s | ts;
                            }
                        }
                    }

                    return s;
                }
            }
        }
    }

    /// <summary>
    /// A response to the Root endpoint.
    /// </summary>
    public class TwitchRoot_Response
    {
        public TwitchToken Token { get; set; }
        public Dictionary<string, string> _links { get; set; }
    }
}

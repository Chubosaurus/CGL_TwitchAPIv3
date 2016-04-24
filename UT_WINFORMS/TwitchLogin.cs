using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Web;

namespace UT_WINFORMS
{
    public partial class TwitchLogin : Form
    {
        public TwitchLogin()
        {
            InitializeComponent();

            this.TwitchCallbackUri = new Uri("http://www.chubosaurus.com");
        }

        private void TwitchBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // check to see if we at the callback URL stage
            if (e.Url.AbsoluteUri.StartsWith(this.TwitchCallbackUri.OriginalString))
            {
                // check the callback #
                int sharp_index = e.Url.AbsoluteUri.IndexOf('#', this.TwitchCallbackUri.OriginalString.Length);

                // got the # location
                if (sharp_index >= 0)
                {
                    // parse the query string
                    string query = e.Url.AbsoluteUri.Substring(sharp_index + 1);

                    // get all the options from the query string
                    var options = HttpUtility.ParseQueryString(query);

                    // check to see if we have the access_token & scope
                    if (options.AllKeys.Contains("access_token") &&
                       options.AllKeys.Contains("scope"))
                    {
                        // create the event
                        OAuthEvent oae = new OAuthEvent();
                        oae.StatusCode = OAuthStatusCode.AUTHENTICATED;
                        oae.OAuth = options["access_token"];

                        // add each scope option
                        string[] scope_options = options["scope"].Split('+');
                        foreach (string scope_option in scope_options)
                        {
                            oae.Scope.Add(scope_option);
                        }

                        // fire the event
                        OnAuthChanged(oae);

                        // clean up
                        this.Hide();
                    }
                }
                else
                {
                    // they cancel the OAuth process by click "cancel" or an error has happen                    
                    // create the event (error)
                    OAuthEvent oae = new OAuthEvent();
                    var options = HttpUtility.ParseQueryString(e.Url.Query);

                    // assume canceled
                    oae.StatusCode = OAuthStatusCode.CANCELED;
                    
                    // check for other error codes
                    if (options.AllKeys.Contains("error"))
                    {
                        if (options["error"] == "redirect_mismatch")
                        {
                            oae.StatusCode = OAuthStatusCode.CALLBACK_URL_MISMATCH;
                            oae.StatusMessage = options["error_description"].Replace('+', ' ');
                        }
                        
                    }
                    
                    this.Hide();
                }
            }
            else if (e.Url.AbsoluteUri.StartsWith("https://api.twitch.tv/"))
            {
                this.Show();
            }
        }

        public delegate void OAuthEventHandler(object sender, OAuthEvent e);

        private event OAuthEventHandler _on_oauth_changed;

        [Category("Twitch Events")]
        [Description("Raised when the OAuth key has changed.")]
        public event OAuthEventHandler OAuthChanged
        {
            add { _on_oauth_changed += value; }
            remove { _on_oauth_changed -= value; }
        }

        protected virtual void OnAuthChanged(OAuthEvent e)
        {
            this.OAuthToken = e.OAuth;

            // do callback
            if (_on_oauth_changed != null)
            {
                _on_oauth_changed.Invoke(this, e);
            }
        }

        private string _oauth_token;
        public string OAuthToken
        {
            get { return _oauth_token; }
            set
            {
                if (_oauth_token != value)
                {
                    _oauth_token = value;
                }
            }
        }

        private Uri _twitch_login_uri;
        public Uri TwitchLoginUri
        {
            get { return _twitch_login_uri; }
            set
            {
                if (_twitch_login_uri != value)
                {
                    _twitch_login_uri = value;
                    TwitchBrowser.Navigate(_twitch_login_uri);
                }
            }
        }

        private Uri _twitch_callback_uri;
        public Uri TwitchCallbackUri
        {
            get { return _twitch_callback_uri; }
            set
            {
                if (_twitch_callback_uri != value)
                {
                    _twitch_callback_uri = value;
                }
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public enum OAuthStatusCode { AUTHENTICATED, CALLBACK_URL_MISMATCH, BAD_LOGIN, CANCELED, UNKNOWN };

    public class OAuthEvent : EventArgs
    {
        public OAuthEvent()
        {
            OAuth = null;
            Scope = new List<string>();
            StatusCode = OAuthStatusCode.UNKNOWN;
            StatusMessage = null;
        }

        public string OAuth { get; set; }
        public List<string> Scope { get; set; }
        public OAuthStatusCode StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }
        

}

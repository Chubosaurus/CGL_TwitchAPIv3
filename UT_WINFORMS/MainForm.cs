using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CGL;
using CGL.TwitchModels;

namespace UT_WINFORMS
{
    public partial class MainForm : Form
    {
        WINFORMS_TwitchAPIv3 TwitchAPI = new WINFORMS_TwitchAPIv3();

        public MainForm()
        {
            InitializeComponent();
            SetupTwitchAPI();
        }

        /// <summary>
        /// Setup TwitchAPI to use developer's information.
        /// </summary>
        private void SetupTwitchAPI()
        {
            TwitchAPI.BaseApiUri = new Uri("https://api.twitch.tv/kraken/");
            TwitchAPI.ClientId = "YOUR_CLIENT_ID";
            TwitchAPI.TwitchCallbackUri = new Uri("http://localhost");
            TwitchAPI.ChannelName = "YOUR_CHANNEL";
        }

        /// <summary>
        /// Start the Unit Test(s).
        /// </summary>
        private async void StartUnitTests_Button_Click(object sender, EventArgs e)
        {
            // check busy flag
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            TwitchAPI_Output.Clear();

            //T NAME = DoUnitTest<T>(() => TwitchAPI.FUNC, "NAME", UpdateStatus);

            TwitchAPI_Output.AppendText("Starting Tests...\r\n");

            // NOTE(duan): determine if the OAUTH is valid (this is key)
            TwitchRoot_Response GET_ROOT = await DoUnitTest<TwitchRoot_Response>(() => TwitchAPI.GetRoot(), "GET_ROOT", UpdateStatus);

            TwitchChannel_Response GET_CHANNEL_INFO = await DoUnitTest<TwitchChannel_Response>(() => TwitchAPI.GetChannelInfo(TwitchAPI.ChannelName), "GET_CHANNEL_INFO", UpdateStatus);
            TwitchChannel_Response GET_AUTH_CHANNEL_INFO = await DoUnitTest<TwitchChannel_Response>(() => TwitchAPI.GetAuthenticatedChannelInfo(), "GET_AUTH_CHANNEL_INFO", UpdateStatus);
            TwitchVideoList_Response GET_CHANNEL_VIDEO = await DoUnitTest<TwitchVideoList_Response>(() => TwitchAPI.GetChannelVideos(TwitchAPI.ChannelName), "GET_CHANNEL_VIDEOS", UpdateStatus);
            TwitchFollowList_Response GET_CHANNEL_FOLLOWER = await DoUnitTest<TwitchFollowList_Response>(() => TwitchAPI.GetChannelFollows(TwitchAPI.ChannelName), "GET_CHANNEL_FOLLOWER", UpdateStatus);
            TwitchEditorList_Response GET_CHANNEL_EDITOR = await DoUnitTest<TwitchEditorList_Response>(() => TwitchAPI.GetChannelEditors(), "GET_CHANNEL_EDITORS", UpdateStatus);

            // NOTE(duan): no call to get the status of the channel feed.
            // NOTE(duan): on the Unit Test we assume, no change
            TwitchChannel_Response PUT_CHANNEL_PROPERTY = null;
            if (GET_AUTH_CHANNEL_INFO != null)
            {
                PUT_CHANNEL_PROPERTY = await DoUnitTest<TwitchChannel_Response>(
                    () => TwitchAPI.UpdateChannelProperties(GET_AUTH_CHANNEL_INFO.status, GET_AUTH_CHANNEL_INFO.game, null, null), "PUT_CHANNEL_PROPERTY", UpdateStatus);
            }
            else
            {
                // TODO(duan): log
            }

            // NOTE(duan): always fail because we're not parterned
            bool POST_START_COMMERICIAL = await DoUnitTest<bool>(() => TwitchAPI.StartCommercial(), "POST_START_COMMERICIAL", UpdateStatus);

            // TODO(duan): test this offstream, probably gonnna wreck stream if online           
            // TwitchChannel_Response DELETE_STREAM_KEY = await DoUnitTest<TwitchChannel_Response>(()=> TwitchAPI.ResetChannelStreamKey(), "DELETE_STREAM_KEY", UpdateStatus);
            TwitchTeamList_Response GET_TEAM_LIST = await DoUnitTest<TwitchTeamList_Response>(() => TwitchAPI.GetChannelTeams(TwitchAPI.ChannelName), "GET_TEAM_LIST", UpdateStatus);


            TwitchChannelFollowList_Response GET_USER_CHANNEL_FOLLOW = await DoUnitTest<TwitchChannelFollowList_Response>(() => TwitchAPI.GetUserChannelFollows(TwitchAPI.ChannelName), "GET_USER_CHANNEL_FOLLOW", UpdateStatus);
            bool GET_USER_CHANNEL_RELATIONSHIP = await DoUnitTest<bool>(() => TwitchAPI.GetUserChannelRelationship("chubosaurus", "mrmiyagi00"), "GET_USER_CHANNEL_RELATIONSHIP", UpdateStatus);
            // NOTE(duan): To test failure uncomment
            //bool GET_USER_CHANNEL_RELATIONSHIP = await DoUnitTest<bool>(() => TwitchAPI.GetUserChannelRelationship("chubosaurus", "this_channel_does_not_exist_so_i_can_t_be_subbed...lasdjflaskdfjalsdkfjlasdkfjklasdjflaskdjflaksdfjlasdfj;asdklfjasdkljf"), "GET_USER_CHANNEL_RELATIONSHIP", UpdateStatus);
            

            // NOTE(duan): to test follow and unfollow we need a USER_CHANNEL relationship of false
            bool TEST_PUT_FOLLOW_CHANNEL = await DoUnitTest<bool>(() => TwitchAPI.GetUserChannelRelationship("chubosaurus", "c9sneaky"), "GET_USER_CHANNEL_RELATIONSHIP/FAIL", UpdateStatus);
            if (!TEST_PUT_FOLLOW_CHANNEL)
            {
                // follow the channel
                bool PUT_FOLLOW_CHANNEL = await DoUnitTest<bool>(() => TwitchAPI.FollowChannel("c9sneaky"), "PUT_FOLLOW_CHANNEL", UpdateStatus);
                // unfollow the channel
                bool DELETE_FOLLOW_CHANNEL = await DoUnitTest<bool>(() => TwitchAPI.UnFollowChannel("c9sneaky"), "DELETE_FOLLOW_CHANNEL", UpdateStatus);
            }

            TwitchGamesList_Response GET_TOP_GAMES = await DoUnitTest<TwitchGamesList_Response>(() => TwitchAPI.GetTopGames(), "GET_TOP_GAMES", UpdateStatus);

            TwitchIngestsList_Response GET_INGEST = await DoUnitTest<TwitchIngestsList_Response>(() => TwitchAPI.GetIngests(), "GET_INGEST", UpdateStatus);

            TwitchSearchChannelsList_Response GET_SEARCH_CHANNEL = await DoUnitTest<TwitchSearchChannelsList_Response>(() => TwitchAPI.SearchChannels("FOOD"), "GET_SEARCH_CHANNEL", UpdateStatus);
            TwitchSearchStreamList_Response GET_SEARCH_STREAM = await DoUnitTest<TwitchSearchStreamList_Response>(() => TwitchAPI.SearchStreams("FOOD"), "GET_SEARCH_STREAM", UpdateStatus);
            TwitchSearchGamesList_Response GET_SEARCH_GAME = await DoUnitTest<TwitchSearchGamesList_Response>(() => TwitchAPI.SearchGames("Star"), "GET_SEARCH_GAME", UpdateStatus);

            TwitchBlockedUser_Response ADD_BLOCK_USER = await DoUnitTest<TwitchBlockedUser_Response>(() => TwitchAPI.AddUserToBlockList("agriasz"), "ADD_BLOCK_USER", UpdateStatus);
            TwitchBlockUserList_Response GET_BLOCK_LIST = await DoUnitTest<TwitchBlockUserList_Response>(() => TwitchAPI.GetUserBlockList(), "GET_BLOCK_LIST", UpdateStatus);
            bool REM_BLOCK_USER = await DoUnitTest<bool>(() => TwitchAPI.DeleteUserFromBlockList("agriasz"), "REM_BLOCK_USER", UpdateStatus);

            TwitchPostList_Response GET_CHANNEL_POST = await DoUnitTest<TwitchPostList_Response>(() => TwitchAPI.GetChannelPosts("chubosaurus"), "GET_CHANNEL_POST", UpdateStatus);
            TwitchPostList_Response GET_AUTH_CHANNEL_POST = await DoUnitTest<TwitchPostList_Response>(() => TwitchAPI.GetAuthenticatedChannelPosts(), "GET_AUTH_CHANNEL_POST", UpdateStatus);
            TwitchPostTweet_Response POST_CHANNEL_POST = await DoUnitTest<TwitchPostTweet_Response>(() => TwitchAPI.PostChannelPost(string.Format("TESTING API@{0}", DateTime.Now.ToShortDateString())), "POST_CHANNEL_POST", UpdateStatus);

            TwitchPost_Response GET_CHANNEL_POST_BY_ID = null;
            TwitchPost_Response GET_CHANNEL_AUTH_POST_BY_ID = null;
            TwitchPost_Response DELETE_CHANNEL_POST_BY_ID = null;
            TwitchPostReaction_Response POST_CHANNEL_POST_REACTION = null;
            
            // NOTE(duan): get the status of the stream
            TwitchStream_Repsonse GET_CHANNEL_STREAM = await DoUnitTest<TwitchStream_Repsonse>(() => TwitchAPI.GetChannelStream(TwitchAPI.ChannelName), "GET_CHANNEL_STREAM", UpdateStatus);
            // NOTE(duan): To test offline stream uncomment (check the stream object of the response == null)
            // TwitchStream_Repsonse GET_CHANNEL_STREAM_OFFLINE = await DoUnitTest<TwitchStream_Repsonse>(() => TwitchAPI.GetChannelStream("OFFLINE_CHANNEL"), "GET_CHANNEL_STREAM", UpdateStatus);

            TwitchStreamList_Response GET_ALL_GAMES_BY_TITLE = await DoUnitTest<TwitchStreamList_Response>(() => TwitchAPI.GetAllStreamByGame("Destiny"), "GET_ALL_GAMES_BY_TITLE", UpdateStatus);
            TwitchStreamFeaturedList_Response GET_ALL_FEATURED_STEAMS = await DoUnitTest<TwitchStreamFeaturedList_Response>(() => TwitchAPI.GetAllFeaturedStreams(), "GET_ALL_FEATURED_STEAMS", UpdateStatus);
            TwitchStreamSummary_Response GET_TWITCH_STREAM_STATUS = await DoUnitTest<TwitchStreamSummary_Response>(() => TwitchAPI.GetStreamSummary(), "GET_TWITCH_STREAM_STATUS", UpdateStatus);

            // NOTE(duan): SUBSCRIPTIONS TEST METHODS ARE A NO GO.
            // NOTE(duan): we need to find a partnered channel to test
            // TODO(duan): SUBSCRIPTIONS UnitTest(s)

            TwitchTeamList_Response GET_PUBLIC_TEAM_LIST = await DoUnitTest<TwitchTeamList_Response>(() => TwitchAPI.GetTeamList(), "GET_PUBLIC_TEAM_LIST", UpdateStatus);
            if (GET_PUBLIC_TEAM_LIST != null)
            {
                if(GET_PUBLIC_TEAM_LIST.teams.Count > 0)
                {
                    // test getting information for a specific team
                    TwitchTeam_Response GET_PUBLIC_TEAM_INFO = await DoUnitTest<TwitchTeam_Response>(
                        () => TwitchAPI.GetTeam(GET_PUBLIC_TEAM_LIST.teams[0].name), "GET_PUBLIC_TEAM_INFO", UpdateStatus
                        );
                }
            }

            TwitchUser GET_PUBLIC_USER = await DoUnitTest<TwitchUser>(() => TwitchAPI.GetUserInfo(TwitchAPI.ChannelName), "GET_PUBLIC_USER", UpdateStatus);
            TwitchAuthenticatedUser GET_AUTH_USER = await DoUnitTest<TwitchAuthenticatedUser>(() => TwitchAPI.GetAuthenticatedUserInfo(), "GET_AUTH_USER", UpdateStatus);

            TwitchStreamList_Response GET_AUTH_USER_FOLLOWS = await DoUnitTest<TwitchStreamList_Response>(() => TwitchAPI.GetUserFollowList(25, 0, "playlist"), "GET_AUTH_USER_FOLLOWS", UpdateStatus);

            TwitchVideoList_Response GET_AUTH_USER_VIDEO_FOLLOWS = await DoUnitTest<TwitchVideoList_Response>(() => TwitchAPI.GetUserVideoFollows(), "GET_AUTH_USER_VIDEO_FOLLOWS ", UpdateStatus);

            TwitchVideoList_Response GET_TOP_VIDEOS = await DoUnitTest<TwitchVideoList_Response>(() => TwitchAPI.GetTopVideos(), "GET_TOP_VIDEOS", UpdateStatus);

            if (GET_TOP_VIDEOS != null)
            {
                // test getting information on a single video from a video_id
                if(GET_TOP_VIDEOS.videos.Count > 0)
                {
                    TwitchVideo_Response GET_VIDEO_INFO = await DoUnitTest<TwitchVideo_Response>(
                        () => TwitchAPI.GetVideo(GET_TOP_VIDEOS.videos[0]._id), "GET_VIDEO_INFO", UpdateStatus
                        );
                }
            }

            if (POST_CHANNEL_POST != null)
            {
                GET_CHANNEL_POST_BY_ID = await DoUnitTest<TwitchPost_Response>(() => TwitchAPI.GetPostById(TwitchAPI.ChannelName, POST_CHANNEL_POST.Post.Id), "GET_CHANNEL_POST", UpdateStatus);
                GET_CHANNEL_AUTH_POST_BY_ID = await DoUnitTest<TwitchPost_Response>(() => TwitchAPI.GetAuthenticatedPostById(POST_CHANNEL_POST.Post.Id), "GET_CHANNEL_AUTH_POST_BY_ID", UpdateStatus);
                POST_CHANNEL_POST_REACTION = await DoUnitTest<TwitchPostReaction_Response>(() => TwitchAPI.PostChannelReaction(TwitchAPI.ChannelName, POST_CHANNEL_POST.Post.Id), "POST_CHANNEL_POST_REACTION", UpdateStatus);
                bool DELETE_CHANNEL_POST_REACTION = await DoUnitTest<bool>(() => TwitchAPI.DeleteChannelReaction(TwitchAPI.ChannelName, POST_CHANNEL_POST.Post.Id), "DELETE_CHANNEL_POST_REACTION", UpdateStatus);
                DELETE_CHANNEL_POST_BY_ID = await DoUnitTest<TwitchPost_Response>(() => TwitchAPI.DeletePostById(POST_CHANNEL_POST.Post.Id), "DELETE_CHANNEL_POST_BY_ID", UpdateStatus);
            }


            TwitchLinks GET_CHAT_ENDPOINTS = await DoUnitTest<TwitchLinks>(() => TwitchAPI.GetChatEndPoints(), "GET_CHAT_ENDPOINTS", UpdateStatus);
            TwitchBadge_Response GET_CHAT_BADGES = await DoUnitTest<TwitchBadge_Response>(() => TwitchAPI.GetChatBadges(), "GET_CHAT_BADGES", UpdateStatus);
            TwitchEmoticonList_Response GET_CHAT_EMOTICONS = await DoUnitTest<TwitchEmoticonList_Response>(() => TwitchAPI.GetChatEmoticons(), "GET_CHAT_EMOTICONS", UpdateStatus);
            TwitchEmoticonImageList_Response GET_CHAT_EMOTICON_IMAGES = await DoUnitTest<TwitchEmoticonImageList_Response>(() => TwitchAPI.GetChatEmoticonImages(), "GET_CHAT_EMOTICON_IMAGES", UpdateStatus);
            TwitchEmoticonSet_Response GET_CHAT_EMOTICON_SET = await DoUnitTest<TwitchEmoticonSet_Response>(() => TwitchAPI.GetChatEmoticonSet("0,496"), "GET_CHAT_EMOTICON_SET", UpdateStatus);
            IsBusy = false;
        }

        /// <summary>
        /// Update the output box with status.
        /// </summary>
        /// <param name="input">The input object (either a response from an operation, or a status title").</param>
        /// <param name="mode">The mode (0 = Status Title, 1 = Response from an operation)</param>
        /// <returns>Return the status of the calling operation.</returns>
        private bool UpdateStatus(object input, int mode = 0)
        {
            switch (mode)
            {
                case 0:
                    TwitchAPI_Output.AppendText(string.Format("{0}...", input.ToString()));
                    return true;
                case 1:
                default:
                    if (input == null)
                    {
                        TwitchAPI_Output.AppendText("[FAIL]\r\n");
                        return false;
                    }
                    else
                    {
                        TwitchAPI_Output.AppendText("[SUCCESS]\r\n");
                        return true;
                    }
            }
        }

        /// <summary>
        /// Helper function to do the Unit Test.
        /// </summary>
        /// <typeparam name="T">The return Type.</typeparam>
        /// <param name="function">The function to call.</param>
        /// <param name="title">The title of the test.</param>
        /// <param name="update_call_back">The call back to perform before and after the operation.</param>
        /// <returns>Returns T on success, else null.</returns>
        private async Task<T> DoUnitTest<T>(Func<Task<T>> function, string title, Func<object, int, bool> update_callback)
        {
            update_callback(title, 0);
            T ret = default(T);
            ret = await Task.Run(function);
            //Task<T> task = Task.Run(function);
            //ret = task.Result;
            

            if (ret is bool)
            {
                if((bool)Convert.ChangeType(ret, typeof(bool)) == true)
                {
                    update_callback(ret, 1);
                }
                else
                {
                    update_callback(null, 1);
                }
            }
            else
            {
                update_callback(ret, 1);
            }            
            return ret;
        }

        /// <summary>
        /// True when the Unit Tests functions are running, else fasle.
        /// </summary>
        public bool IsBusy { get; set; }

        /// <summary>
        /// Get the OAUTH key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetOAuth_Button_Click(object sender, EventArgs e)
        {
            string new_token = TwitchAPI.GetOAuthToken(TwitchScope.full_control, this);
            if (new_token != null)
            {
                TwitchAPI.OAuthToken = new_token;
            }
        }
    }

    /// <summary>
    /// A TwitchAPIv3 with WINFORMS OAUTH version 2 pass through
    /// </summary>
    public class WINFORMS_TwitchAPIv3 : CGL.TwitchAPIv3
    {
        /// <summary>
        /// Override for WINFORM OAUTH version 2.
        /// </summary>
        /// <param name="scope">The permission requested.</param>
        /// <param name="parent">The parent form.</param>
        /// <returns>The OAUTH token if successful, else null.</returns>
        public override string GetOAuthToken(TwitchScope scope = TwitchScope.chat_login, object parent = null)
        {
            // create the login
            if (_twitch_login_page == null)
            {
                _twitch_login_page = new TwitchLogin();
            }
            if (parent != null)
            {
                if (parent is Form)
                {
                    _twitch_login_page.Owner = parent as Form;
                }
            }

            _twitch_login_page.TwitchCallbackUri = this.TwitchCallbackUri;
            _twitch_login_page.TwitchLoginUri = new Uri(BaseApiUri,
                    string.Format("oauth2/authorize?force_verify=true&response_type=token&client_id={0}&redirect_uri={1}&scope={2}",
                    this.ClientId, this.TwitchCallbackUri.OriginalString, GetScopeString(scope)));
            _twitch_login_page.ShowDialog();

            // set the OAuth and the Scope
            this.OAuthToken = _twitch_login_page.OAuthToken;
            if (_twitch_login_page.OAuthToken != null)
            {
                this.TwitchScope = scope;
            }

            return _twitch_login_page.OAuthToken;
        }

        private TwitchLogin _twitch_login_page;
    }
}

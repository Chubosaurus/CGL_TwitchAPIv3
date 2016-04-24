using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using CGL.TwitchModels;

namespace CGL
{
    public class TwitchAPIv3
    {
        public TwitchAPIv3()
        {
            SetupTwitchAPIv3();
        }

        // NOTE(duan): see the WINFORMS Unit Tests for an example on how to build a custom form
        // to get the this working.

        /// <summary>
        /// Get the OAuthToken on your choosen platform.  You need to override this and come up with your own UI to
        /// get the user to accept the OAUTH version 2 pass through.
        /// </summary>
        /// <param name="scope">The requested SCOPE (Twitch Permissions)</param>
        /// <param name="parent">The parent UI object if any.</param>
        /// <returns>Returns null.</returns>
        public virtual string GetOAuthToken(TwitchScope scope = TwitchScope.chat_login, object parent = null)
        {
            return null;
        }


        /// <summary>
        /// Setup the TwitchAPIv3 with default values.
        /// </summary>
        public void SetupTwitchAPIv3()
        {
            this.BaseApiUri = new Uri("https://api.twitch.tv/kraken/");
            this.TwitchCallbackUri = new Uri("http://localhost");
            this.OAuthToken = null;
            this.TwitchScope = TwitchScope.none;
        }

        /// <summary>
        /// Get the scope string to path to the OAuth function to request permissions.
        /// </summary>
        /// <param name="scope">The scope(s) you want access to.</param>
        /// <returns>A string of scope.</returns>
        protected string GetScopeString(TwitchScope scope = TwitchScope.chat_login)
        {
            string ret = "";

            int count = 0;
            foreach (TwitchScope ts in Enum.GetValues(typeof(TwitchScope)))
            {
                // skip full_control
                if (ts == TwitchScope.full_control || ts == TwitchScope.none)
                    continue;

                // check the bit, if the bit is set add it to the return value
                if ((scope & ts) == ts)
                {
                    if (count == 0)
                    {
                        ret += ts.ToString();
                    }
                    else
                    {
                        ret += "+" + ts.ToString();
                    }
                }
                count++;
            }

            return ret;
        }

        #region [Blocks Functions --------------------------------------------]

        /// <summary>
        /// Get the User's BlockList.
        /// </summary>
        /// <param name="limit">Maximum number of objects in array. Default is 25. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <returns>TwitchBlockUserList_Response of users that are block on success, else null.</returns>
        public async Task<TwitchBlockUserList_Response> GetUserBlockList(int limit = 25, int offset = 0)
        {
            return await HttpAction<TwitchBlockUserList_Response>(new Uri(BaseApiUri, string.Format("users/{0}/blocks?limit={1}&offset{2}", this.ChannelName, limit, offset)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.GET);
        }

        /// <summary>
        /// Add the user to the block list.
        /// </summary>
        /// <param name="name">The name of the user to block.</param>
        /// <returns>Returns the user object if successful, else null.</returns>
        public async Task<TwitchBlockedUser_Response> AddUserToBlockList(string name = null)
        {
            return await HttpAction<TwitchBlockedUser_Response>(new Uri(BaseApiUri, string.Format("users/{0}/blocks/{1}", this.ChannelName, name)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.PUT);
        }

        /// <summary>
        /// Delete a user from the block list.
        /// </summary>
        /// <param name="name">The user to unblock.</param>
        /// <returns>Returns true if the user was removed from the block list, else false.</returns>
        public async Task<bool> DeleteUserFromBlockList(string name)
        {
            // null and length checks
            if (string.IsNullOrEmpty(this.ChannelName) || string.IsNullOrEmpty(name))
            {
                return false;
            }            

            return await HttpAction<bool>(new Uri(BaseApiUri, string.Format("users/{0}/blocks/{1}", this.ChannelName, name)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.DELETE);
        }

        #endregion

        #region [Channel Feed Functions --------------------------------------]

        /// <summary>
        /// Returns a list of posts that belong to the channel's feed. Uses limit and cursor pagination.
        /// </summary>
        /// <param name="channel">The channel name to query from.</param>
        /// <param name="limit">Maximum number of objects in array. Default is 10. Maximum is 100.</param>
        /// <param name="cursor">Cursor value to begin next page</param>
        /// <returns>A TwitchPostList_Response of all Channel's Posts on success, else null.</returns>
        public async Task<TwitchPostList_Response> GetChannelPosts(string channel = null, int limit = 10, string cursor = null)
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(channel))
            {
                return null;
            }

            string uri_part = String.Format("feed/{0}/posts?limit={1}{2}", channel, limit,
                String.Format("{0}", (string.IsNullOrEmpty(cursor)) ? "" : string.Format("&cursor={0}", cursor))
                );

            return await HttpAction<TwitchPostList_Response>(new Uri(BaseApiUri, uri_part), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Returns a list of posts that belong to the channel's feed. Uses limit and cursor pagination.
        /// </summary>
        /// <param name="limit">Maximum number of objects in array. Default is 10. Maximum is 100.</param>
        /// <param name="cursor">Cursor value to begin next page</param>
        /// <returns>A TwitchPostList_Response of all Channel's Posts on success, else null.</returns>
        public async Task<TwitchPostList_Response> GetAuthenticatedChannelPosts(int limit = 10, string cursor = null)
        {
            // null and length check
            if (string.IsNullOrWhiteSpace(this.ChannelName))
            {
                return null;
            }

            string uri_part = String.Format("feed/{0}/posts?limit={1}{2}", this.ChannelName, limit,
                String.Format("{0}", (string.IsNullOrEmpty(cursor)) ? "" : string.Format("&cursor={0}", cursor))
                );

            return await HttpAction<TwitchPostList_Response>(new Uri(BaseApiUri, uri_part), AUTHENTICATION.AUTHENTICATED, REST_ACTION.GET);
        }

        /// <summary>
        /// Post a message to channel's feed.
        /// </summary>
        /// <param name="post">The message to post.</param>
        /// <returns>A TwitchPostTweet representing the post.</returns>
        public async Task<TwitchPostTweet_Response> PostChannelPost(string post = null, bool share = false)
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(this.ChannelName) || string.IsNullOrWhiteSpace(post))
            {
                return null;
            }

            // POST Data
            FormUrlEncodedContent url_encoded = new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("content", post)
                });

            return await HttpAction<TwitchPostTweet_Response>(new Uri(BaseApiUri, string.Format("feed/{0}/posts?share={1}", this.ChannelName, share.ToString().ToLower())), AUTHENTICATION.AUTHENTICATED, REST_ACTION.POST, url_encoded);
        }

        /// <summary>
        /// Get the Post by Id number.
        /// </summary>
        /// <param name="channel">The channel name to query the Post from.</param>
        /// <param name="id">The Id of the Post.</param>
        /// <returns>A TwitchPost if successful, else null.</returns>
        public async Task<TwitchPost_Response> GetPostById(string channel = null, System.UInt64 id = 0)
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(channel))
            {
                return null;
            }

            return await HttpAction<TwitchPost_Response>(new Uri(BaseApiUri, string.Format("feed/{0}/posts/{1}", channel, id)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Get the Authenticated Post by Id number.
        /// </summary>
        /// <param name="id">The Id of the Post.</param>
        /// <returns>A TwitchPost if successful, else null.</returns>
        public async Task<TwitchPost_Response> GetAuthenticatedPostById(System.UInt64 id = 0)
        {
            // null and length checks
            if (string.IsNullOrEmpty(this.ChannelName))
            {
                return null;
            }

            return await HttpAction<TwitchPost_Response>(new Uri(BaseApiUri, string.Format("feed/{0}/posts/{1}", this.ChannelName, id)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.GET);
        }

        /// <summary>
        /// Delete a Post by Id number
        /// </summary>
        /// <param name="id">The Id of the Post.</param>
        /// <returns>A TwitchPost if successful, else null.  Check IsDeleted Property to make sure if it was really deleted.</returns>
        public async Task<TwitchPost_Response> DeletePostById(System.UInt64 id = 0)
        {
            return await HttpAction<TwitchPost_Response>(new Uri(BaseApiUri, string.Format("feed/{0}/posts/{1}", this.ChannelName, id)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.DELETE);
        }

        /// <summary>
        /// Post a Reaction (+1 or emote) to a Channel Post.
        /// </summary>
        /// <param name="channel">The channel the Post belongs to.</param>
        /// <param name="id">The Id of the Post.</param>
        /// <param name="emote">The emote to send, if +1 then the emote = "endorse"</param>
        /// <returns>A TwitchPostReaction_Response if successul, else null.</returns>
        public async Task<TwitchPostReaction_Response> PostChannelReaction(string channel = null, System.UInt64 id = 0, string emote = "endorse")
        {
            // null and length checks
            if (string.IsNullOrEmpty(channel))
            {
                return null;
            }

            return await HttpAction<TwitchPostReaction_Response>(new Uri(BaseApiUri, string.Format("feed/{0}/posts/{1}/reactions?emote={2}", channel, id, emote)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.POST);
        }

        /// <summary>
        /// Delete a Reaction from a Channel Post.
        /// </summary>
        /// <param name="channel">The channel the Post belongs to.</param>
        /// <param name="id">The Id of the Post.</param>
        /// <returns>True if successful, else false.</returns>
        public async Task<bool> DeleteChannelReaction(string channel = null, System.UInt64 id = 0)
        {
            // null and length checks
            if (string.IsNullOrEmpty(channel))
            {
                return false;
            }

            TwitchDeleteReaction_Response response = await HttpAction<TwitchDeleteReaction_Response>(new Uri(BaseApiUri, string.Format("feed/{0}/posts/{1}/reactions", channel, id)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.DELETE);

            if (response == null)
            {
                return false;
            }
            else
            {
                if (response.Deleted)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion

        #region [Channel Functions -------------------------------------------]

        /// <summary>
        /// Get the channel's information.
        /// </summary>
        /// <param name="channel">The channel to get the information from.</param>
        /// <returns>A TwitchChannel_Response on success, else null.</returns>
        public async Task<TwitchChannel_Response> GetChannelInfo(string channel = null)
        {
            // null check and length check
            if (string.IsNullOrEmpty(channel))
            {
                return null;
            }

            return await HttpAction<TwitchChannel_Response>(new Uri(BaseApiUri, string.Format("channels/{0}", this.ChannelName)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        
        /// <summary>
        /// Get the Authenticated channel's information.
        /// </summary>
        /// <returns>A TwitchChannel_Response on success, else null.</returns>
        public async Task<TwitchChannel_Response> GetAuthenticatedChannelInfo()
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(this.OAuthToken) || string.IsNullOrWhiteSpace(this.ChannelName))
            {
                return null;
            }

            return await HttpAction<TwitchChannel_Response>(new Uri(BaseApiUri, string.Format("channel")), AUTHENTICATION.AUTHENTICATED, REST_ACTION.GET);
        }
        

        /// <summary>
        /// Get the Channel's video(s).
        /// </summary>
        /// <param name="channel">The channel to get the videos from.</param>
        /// <param name="limit">Maximum number of objects in array. Default is 10. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="broadcast">Returns only broadcasts when true. Otherwise only highlights are returned. Default is false.</param>
        /// <param name="hls">Returns only HLS VoDs when true. Otherwise only non-HLS VoDs are returned. Default is false.</param>
        /// <returns>A TwitchVideoList_Response of the channel's video on success, else null.</returns>
        public async Task<TwitchVideoList_Response> GetChannelVideos(string channel = null, int limit = 10, int offset = 0, bool broadcast = false, bool hls = false)
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(channel))
            {
                return null;
            }

            return await HttpAction<TwitchVideoList_Response>(new Uri(BaseApiUri, string.Format("channels/{0}/videos?limit={1}&offset={2}&broadcast={3}&hls={4}", channel, limit, offset, broadcast, hls)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Get the channel's list of followers.
        /// </summary>
        /// <param name="channel">The channel name to get the list of followers from.</param>
        /// <param name="limit">Maximum number of objects in array. Default is 25. Maximum is 100.</param>
        /// <param name="cursor">Twitch uses cursoring to paginate long lists of followers. Check _cursor in response body and set cursor to this value to get the next page of results, or use _links.next to navigate to the next page of results.</param>
        /// <param name="direction">Creation date sorting direction. Default is desc. Valid values are asc and desc.</param>
        /// <returns>A TwitchFollowList_Response of the channel's followers on success, else null.</returns>
        public async Task<TwitchFollowList_Response> GetChannelFollows(string channel = null, int limit = 25, string cursor = null, string direction = "desc")
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(channel))
            {
                return null;
            }

            return await HttpAction<TwitchFollowList_Response>(new Uri(BaseApiUri, string.Format("channels/{0}/follows?limit={1}&direction={2}", channel, limit, direction)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        
        /// <summary>
        /// Get the list of the authenticated channel's editors.
        /// </summary>
        /// <returns>A TwitchEditorList_Response of editors on success, else null.</returns>
        public async Task<TwitchEditorList_Response> GetChannelEditors()
        {
            return await HttpAction<TwitchEditorList_Response>(new Uri(BaseApiUri, string.Format("channels/{0}/editors", this.ChannelName)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.GET);
        }

        /// <summary>
        /// Update channel's properties. 
        /// </summary>
        /// <param name="status">Channel's title.</param>
        /// <param name="game">Channel's title.</param>
        /// <param name="channel_feed_enabled">Whether the channel's feed is enabled. Requires the channel owner's OAuth token.</param>
        /// <param name="delay">Channel delay in seconds. Requires the channel owner's OAuth token.</param>
        /// <returns>A TwitchChannel_Response of the updated channel's properties on success, else null.</returns>
        public async Task<TwitchChannel_Response> UpdateChannelProperties(string status = null, string game = null, bool? channel_feed_enabled = null, string delay = null)
        {
            TwitchChannelProperties channel_properties = new TwitchChannelProperties
            {
                Status = status,
                Game = game,
                Delay = delay,
                ChannelFeedEnabled = channel_feed_enabled
            };

            return await HttpAction<TwitchChannel_Response>(new Uri(BaseApiUri, string.Format("channels/{0}", this.ChannelName)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.PUT, channel_properties);
        }
        

        /// <summary>
        /// Start commercial on channel.
        /// </summary>
        /// <param name="length">Length of commercial break in seconds. Default value is 30. Valid values are 30, 60, 90, 120, 150, and 180. You can only trigger a commercial once every 8 minutes.</param>
        /// <returns>Returns true on successful commercial start, else false.</returns>
        public async Task<bool> StartCommercial(int length = 30)
        {
            // POST Data
            FormUrlEncodedContent url_encoded = new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("length", length.ToString())
                });

            return await HttpAction<bool>(new Uri(BaseApiUri, string.Format("channels/{0}/commercial", this.ChannelName)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.POST, url_encoded);
        }

        
        /// <summary>
        /// Resets channel's stream key.
        /// </summary>
        /// <returns>A TwitchChannel with the updated information from the reset.</returns>
        public async Task<TwitchChannel_Response> ResetChannelStreamKey()
        {
            return await HttpAction<TwitchChannel_Response>(new Uri(BaseApiUri, string.Format("channels/{0}/stream_key", this.ChannelName)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.DELETE);
        }

        /// <summary>
        /// Returns a list of team objects that the channel belongs to.
        /// </summary>
        /// <param name="channel">The channel name to query.</param>
        /// <returns>A TwitchTeamList_Response of teams on success, else null.</returns>
        public async Task<TwitchTeamList_Response> GetChannelTeams(string channel = null)
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(channel))
            {
                return null;
            }

            return await HttpAction<TwitchTeamList_Response>(new Uri(BaseApiUri, string.Format("channels/{0}/teams", this.ChannelName)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        #endregion

        #region [Chat Functions ----------------------------------------------]

        /// <summary>
        /// Get the list of Chat End Points from the channel.
        /// </summary>
        /// <returns>A TwitchLinks of links on success, else null.</returns>
        public async Task<TwitchLinks> GetChatEndPoints(string channel = null)
        {
            // use default channel or not?
            if (string.IsNullOrEmpty(channel))
            {
                if (string.IsNullOrEmpty(this.ChannelName))
                {
                    return null;
                }
                channel = this.ChannelName;
            }

            return await HttpAction<TwitchLinks>(new Uri(BaseApiUri, string.Format("chat/{0}", channel)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Get the list of Chat Badges from the channel.
        /// </summary>
        /// <returns>A TwitchBadge_Response of Badges on success, else null.</returns>
        public async Task<TwitchBadge_Response> GetChatBadges(string channel = null)
        {
            // use default channel or not?
            if (string.IsNullOrEmpty(channel))
            {
                if (string.IsNullOrEmpty(this.ChannelName))
                {
                    return null;
                }
                channel = this.ChannelName;
            }

            TwitchBadge_Response ret = await HttpAction<TwitchBadge_Response>(new Uri(BaseApiUri, string.Format("chat/{0}/{1}", channel, "badges")), AUTHENTICATION.PUBLIC, REST_ACTION.GET);

            // setup the easy access dictionary if successful
            if(ret != null)
            {
                ret.SetupDictionary();
            }

            return ret; 
        }

        /// <summary>
        /// Get a list of all emoticon objects for Twitch. 
        /// </summary>
        /// <returns>A TwitchEmoticon_Response of all Emoticons on success, else null.</returns>
        public async Task<TwitchEmoticonList_Response> GetChatEmoticons()
        {
            return await HttpAction<TwitchEmoticonList_Response>(new Uri(BaseApiUri, string.Format("chat/emoticons")), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Get a list of all emoticon objects for Twitch (this just returns minimum information). 
        /// </summary>
        /// <returns>A TwitchEmoticonImageList_Response of all Images on success, else null.</returns>
        public async Task<TwitchEmoticonImageList_Response> GetChatEmoticonImages()
        {
            return await HttpAction<TwitchEmoticonImageList_Response>(new Uri(BaseApiUri, string.Format("chat/emoticon_images")), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Get a list of Emoticon within a specific set.
        /// </summary>
        /// <param name="emotesets">Emotes from a comma separated list of emote sets.</param>
        /// <returns>A TwitchEmoticonSet_Response of all Emoticons with a specific set Id on success, else false.</returns>
        public async Task<TwitchEmoticonSet_Response> GetChatEmoticonSet(string emotesets = null)
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(emotesets))
            {
                return null;
            }

            return await HttpAction<TwitchEmoticonSet_Response>(new Uri(BaseApiUri, string.Format("chat/emoticon_images?emotesets={0}", emotesets)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        #endregion

        #region [Follows Functions -------------------------------------------]

        /// <summary>
        /// Get the List of channels that the user is following.
        /// </summary>
        /// <param name="username">The username (or channelname).</param>
        /// <param name="limit">Maximum number of objects in array. Default is 25. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="direction">Sorting direction. Default is desc. Valid values are asc and desc.</param>
        /// <param name="sortby">Sort key. Default is created_at.</param>
        /// <returns>A TwitchChannelFollowList_Response of channels that the user is following on success, else null.</returns>
        public async Task<TwitchChannelFollowList_Response> GetUserChannelFollows(string username = null, int limit = 25, int offset = 0, string direction = "desc", string sortby = "created_at")
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            return await HttpAction<TwitchChannelFollowList_Response>(new Uri(BaseApiUri, String.Format("users/{0}/follows/channels?limit={1}&offset={2}&direction={3}&sortby={4}", username, limit, offset, direction, sortby)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Get status of follow relationship between user and target channel.
        /// </summary>
        /// <param name="username">The username to check vs the channel name.</param>
        /// <param name="channel">The channel to check vs the user name.</param>
        /// <returns>True if the the user is following the channel, else false.</returns>
        public async Task<bool> GetUserChannelRelationship(string username = null, string channel = null)
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(channel))
            {
                return false;
            }

            TwitchUserChannelRelationship_Response rep = await HttpAction<TwitchUserChannelRelationship_Response>(new Uri(BaseApiUri, string.Format("users/{0}/follows/channels/{1}", username, channel)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);

            // NOTE(duan): if rep is null then most likely return a 404 error so return false.
            // NOTE(duan): 404 Not Found if user is not following channel.
            if (rep == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Follow a channel.
        /// </summary>
        /// <param name="channel">The channel to follow.</param>
        /// <param name="notifications">Turn notifications on or off (default false).</param>
        /// <returns>True if the channel is followed successfully, else false.</returns>
        // NOTE(duan): if the channel is already followed, it updates the channel with the new value of the notifications
        // NOTE(duan): if channel does not exist, 404ed
        public async Task<bool> FollowChannel(string channel = null, bool notifications = false)
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(channel))
            {
                return false;
            }

            TwitchChannelFollowed_Response rep = await HttpAction<TwitchChannelFollowed_Response>(new Uri(BaseApiUri, string.Format("users/{0}/follows/channels/{1}?notifications={2}", this.ChannelName, channel, notifications.ToString().ToLower())), AUTHENTICATION.AUTHENTICATED, REST_ACTION.PUT);

            if (rep != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Unfollow a channel.
        /// </summary>
        /// <param name="channel">The channel name to unfollow.</param>
        /// <returns>True if the channel is unfollowed, else false.</returns>
        public async Task<bool> UnFollowChannel(string channel = null)
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(channel))
            {
                return false;
            }

            return await HttpAction<bool>(new Uri(BaseApiUri, string.Format("users/{0}/follows/channels/{1}", this.ChannelName, channel)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.DELETE);
        }

        #endregion

        #region [Games Functions ---------------------------------------------]

        /// <summary>
        /// Get the list of the Top Games.
        /// </summary>
        /// <param name="limit">Maximum number of objects in array. Default is 10. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <returns>A TwitchGamesList_Response of top games on success, else null.</returns>
        public async Task<TwitchGamesList_Response> GetTopGames(int limit = 10, int offset = 0)
        {
            return await HttpAction<TwitchGamesList_Response>(new Uri(BaseApiUri, String.Format("games/top?limit={0}&offset={1}", limit, offset)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        #endregion

        #region [Ingests Functions -------------------------------------------]

        /// <summary>
        /// Get a list of Ingests.
        /// </summary>
        /// <returns>TwitchIngestsList_Response of Ingests on success, else null.</returns>
        public async Task<TwitchIngestsList_Response> GetIngests()
        {
            return await HttpAction<TwitchIngestsList_Response>(new Uri(BaseApiUri, "ingests"), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        #endregion

        #region [Root Functions ----------------------------------------------]

        /// <summary>
        /// Get the Root (OAuth Token info and links).
        /// </summary>
        /// <returns>A Root_Respons on success, else null.</returns>
        public async Task<TwitchRoot_Response> GetRoot()
        {
            return await HttpAction<TwitchRoot_Response>(BaseApiUri, AUTHENTICATION.AUTHENTICATED, REST_ACTION.GET);
        }

        #endregion

        #region [Search Functions --------------------------------------------]

        /// <summary>
        /// Search all Channels.
        /// </summary>
        /// <param name="query">A search query (example "food").</param>
        /// <param name="limit">Maximum number of objects in array. Default is 25. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <returns>A List of channels that match the search query, else null.</returns>
        public async Task<TwitchSearchChannelsList_Response> SearchChannels(string query = null, int limit = 25, int offset = 0)
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(query))
            {
                return null;
            }

            return await HttpAction<TwitchSearchChannelsList_Response>(new Uri(BaseApiUri, string.Format("search/channels?q={0}&limit={1}&offset={2}", query, limit, offset)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Search all Streams.
        /// </summary>
        /// <param name="query">A search query (example "food").</param>
        /// <param name="limit">Maximum number of objects in array. Default is 25. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="hls">If set to true, only returns streams using HLS. If set to false, only returns streams that are non-HLS.</param>
        /// <returns>Returns a List of Streams that match the search query on success, else null.</returns>
        public async Task<TwitchSearchStreamList_Response> SearchStreams(string query = null, int limit = 25, int offset = 0, bool? hls = null)
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(query))
            {
                return null;
            }

            string uri_part = String.Format("search/streams?q={0}&limit={1}&offset={2}{3}", query, limit, offset,
                String.Format("{0}", (hls == null) ? "" : string.Format("&hls={0}", hls.Value.ToString().ToLower()))
                );

            return await HttpAction<TwitchSearchStreamList_Response>(new Uri(BaseApiUri, uri_part), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Search all Games.
        /// </summary>
        /// <param name="query">A search query (example "Contra").</param>
        /// <param name="suggest">suggest: Suggests a list of games similar to query, e.g. 'star' query might suggest 'StarCraft II: Wings of Liberty'.</param>
        /// <param name="live">If true, only returns games that are live on at least one channel.</param>
        /// <returns>Returns a List of games that match the search query on success, else null.</returns>
        public async Task<TwitchSearchGamesList_Response> SearchGames(string query = null, string suggest = "suggest", bool live = true)
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(query))
            {
                return null;
            }

            return await HttpAction<TwitchSearchGamesList_Response>(new Uri(BaseApiUri, string.Format("search/games?q={0}&type={1}&live={2}", query, suggest, live.ToString().ToLower())), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        #endregion

        #region [Streams Functions -------------------------------------------]

        /// <summary>
        /// Get the Channel's stream.
        /// </summary>
        /// <param name="channel">The channel to query.</param>
        /// <returns>A TwitchStream on success, else null.</returns>
        /// NOTE(duan): Check stream object for nulls, this means they're offline.
        public async Task<TwitchStream_Repsonse> GetChannelStream(string channel = null)
        {
            // null and length checks
            if (string.IsNullOrEmpty(channel))
            {
                return null;
            }

            return await HttpAction<TwitchStream_Repsonse>(new Uri(BaseApiUri, string.Format("streams/{0}", channel)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Get all Streams by specific values.
        /// </summary>
        /// <param name="game">Streams categorized under game.</param>
        /// <param name="channels">Streams from a comma separated list of channels.</param>
        /// <param name="limit">Maximum number of objects in array. Default is 25. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="client_id">Only shows streams from applications of client_id.</param>
        /// <param name="stream_type">Only shows streams from a certain type. Permitted values: all, playlist, live</param>
        /// <returns>Returns a list of Streams matching the specific params.</returns>
        public async Task<TwitchStreamList_Response> GetAllStreamByGame(string game = null, string channels = null, int limit = 25, int offset = 0, string client_id = null, string stream_type = "all")
        {
            string uri_part = String.Format("streams?limit={0}&offset={1}&stream_type={2}{3}{4}{5}", limit, offset, stream_type,
                String.Format("{0}", (game == null) ? "" : string.Format("&game={0}", game)),
                String.Format("{0}", (channels == null) ? "" : string.Format("&channel={0}", channels)),
                String.Format("{0}", (client_id == null) ? "" : string.Format("&client_id={0}", client_id))
                );

            return await HttpAction<TwitchStreamList_Response>(new Uri(BaseApiUri, uri_part), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Get the list of "featured" streams.
        /// </summary>
        /// <param name="limit">Maximum number of objects in array. Default is 25. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <returns>A List of featured streams on success, else null.</returns>
        public async Task<TwitchStreamFeaturedList_Response> GetAllFeaturedStreams(int limit = 25, int offset = 0)
        {
            return await HttpAction<TwitchStreamFeaturedList_Response>(new Uri(BaseApiUri, string.Format("streams/featured?limit={0}&offset={1}", limit, offset)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Get main stats from all streams.
        /// </summary>
        /// <returns>Returns a summary of current streams.</returns>
        public async Task<TwitchStreamSummary_Response> GetStreamSummary()
        {
            return await HttpAction<TwitchStreamSummary_Response>(new Uri(BaseApiUri, string.Format("streams/summary")), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        // GET /streams/followed
        // NOTE(duan): See User

        #endregion

        #region [Subscriptions Functions -------------------------------------]

        /// <summary>
        /// Get a list of the Authenticated channel's subscribers.
        /// </summary>
        /// <param name="limit">Maximum number of objects in array. Default is 25. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="direction">Creation date sorting direction. Default is asc. Valid values are asc and desc.</param>
        /// <returns>A List of subscribers to the channel, else null.</returns>
        /// NOTE(duan): not sure this actually works since no way to test without a sub channel. 
        public async Task<TwitchSubscriptionList_Response> GetChannelSubscriptions(int limit = 25, int offset = 0, string direction = "asc")
        {
            // null and length checks
            if (string.IsNullOrEmpty(this.ChannelName))
            {
                return null;
            }

            return await HttpAction<TwitchSubscriptionList_Response>(new Uri(BaseApiUri, string.Format("channels/{0}/subscriptions", this.ChannelName)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.GET);
        }


        /// <summary>
        /// Check to see if the user is subscribed to the Authenticated channel.
        /// </summary>
        /// <param name="user">The user name to check.</param>
        /// <returns>True if the user is subscribed, else false.</returns>
        /// NOTE(duan): not sure this actually works since no way to test without a sub channel. 
        public async Task<bool> IsUserSubscribedToChannel(string user = null)
        {
            // null and length checks
            if (string.IsNullOrEmpty(this.ChannelName) || string.IsNullOrEmpty(user))
            {
                return false;
            }

            TwitchSubscription_Response resp = await HttpAction<TwitchSubscription_Response>(new Uri(BaseApiUri, string.Format("channels/{0}/subscriptions/{1}", this.ChannelName, user)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.GET);

            // if no user response then they are not subscribe
            if (resp == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// Get the channel object if the Authenticated is subscribed to the targeted channel.
        /// </summary>
        /// <param name="channel">The channel to target.</param>
        /// <returns>Returns the channel if the user is sub, else null.</returns>
        public async Task<TwitchChannelSub_Response> GetChannelIfSubcribed(string channel = null)
        {
            // null and length checks
            if (string.IsNullOrEmpty(channel))
            {
                return null;
            }
            return await HttpAction<TwitchChannelSub_Response>(new Uri(BaseApiUri, string.Format("users/{0}/subscriptions/{1}", this.ChannelName, channel)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.GET);
        }

        #endregion

        #region [Team Functions ----------------------------------------------]

        /// <summary>
        /// Returns a team object for :team
        /// </summary>
        /// <param name="team"></param>
        /// <returns>A TwitchTeam_Response of the matching team on success, else false.</returns>
        public async Task<TwitchTeam_Response> GetTeam(string team)
        {
            // null and length checks
            if(string.IsNullOrWhiteSpace(team))
            {
                return null;
            }
                
            return await HttpAction<TwitchTeam_Response>(new Uri(BaseApiUri, string.Format("teams/{0}", team)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Returns a list of active teams.
        /// </summary>
        /// <param name="team_name"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns>A TwitchTeamList_Response of active teams on success, else null.</returns>
        public async Task<TwitchTeamList_Response> GetTeamList(int limit = 25, int offset = 0)
        {
            return await HttpAction<TwitchTeamList_Response>(new Uri(BaseApiUri, string.Format("teams?limit={0}&offset={0}", limit, offset)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        #endregion

        #region [User Functions ----------------------------------------------]

        /// <summary>
        /// Get the information of the specific user.
        /// </summary>
        /// <param name="user">The username.</param>
        /// <returns>A TwitchUser on success, else null.</returns>
        public async Task<TwitchUser> GetUserInfo(string user)
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(user))
            {
                return null;
            }

            return await HttpAction<TwitchUser>(new Uri(BaseApiUri, string.Format("users/{0}", user)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Get the information of the specific user.
        /// </summary>
        /// <returns>A TwitchAuthenticatedUser on success, else null.</returns>
        public async Task<TwitchAuthenticatedUser> GetAuthenticatedUserInfo()
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(this.ChannelName))
            {
                return null;
            }

            return await HttpAction<TwitchAuthenticatedUser>(new Uri(BaseApiUri, string.Format("user")), AUTHENTICATION.AUTHENTICATED, REST_ACTION.GET);
        }

        /// <summary>
        /// Get the list of channels the authenticated user is following.
        /// </summary>
        /// <param name="limit">Maximum number of objects in array. Default is 25. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="stream_type">Only shows streams from a certain type. Permitted values: all, playlist, live.</param>
        /// <returns>A TwitchStreamList_Response of followed channels.</returns>
        public async Task<TwitchStreamList_Response> GetUserFollowList(int limit = 25, int offset = 0, string stream_type = null)
        {
            string uri_part = String.Format("streams/followed?limit={0}&offset={1}{2}", limit, offset,
                String.Format("{0}", (string.IsNullOrWhiteSpace(stream_type)) ? "" : string.Format("&stream_type={0}", stream_type))
                );

            return await HttpAction<TwitchStreamList_Response>(new Uri(BaseApiUri, uri_part), AUTHENTICATION.AUTHENTICATED, REST_ACTION.GET);
        }

        /// <summary>
        /// Returns a list of video objects from channels that the authenticated user is following.
        /// </summary>
        /// <param name="limit">Maximum number of objects in array. Default is 10. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <returns>A TwitchVideoList_Response of videos from the authenticated user that are followed.</returns>
        public async Task<TwitchVideoList_Response> GetUserVideoFollows(int limit = 10, int offset = 0)
        {
            return await HttpAction<TwitchVideoList_Response>(new Uri(BaseApiUri, string.Format("videos/followed?limit={0}&offset={1}", limit, offset)), AUTHENTICATION.AUTHENTICATED, REST_ACTION.GET);
        }

        #endregion

        #region [Video Functions ---------------------------------------------]

        /// <summary>
        /// Returns a video object.
        /// </summary>
        /// <param name="video_id">The id of the video.</param>
        /// <returns>Returns a TwitchVideo on success, else null.</returns>
        public async Task<TwitchVideo_Response> GetVideo(string video_id = null)
        {
            // null and length checks
            if (string.IsNullOrWhiteSpace(video_id))
                return null;

            return await HttpAction<TwitchVideo_Response>(new Uri(BaseApiUri, string.Format("videos/{0}", video_id)), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        /// <summary>
        /// Returns a list of videos created in a given time period sorted by number of views, most popular first.
        /// </summary>
        /// <param name="limit">Maximum number of objects in array. Default is 10. Maximum is 100.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="game">Returns only videos from game.</param>
        /// <param name="period">Returns only videos created in time period. Valid values are week, month, or all. Default is week.</param>
        /// <returns>Returns a list of videos on success, else null.</returns>
        public async Task<TwitchVideoList_Response> GetTopVideos(int limit = 10, int offset = 0, string game = null, string period = "week")
        {
            string uri_part = String.Format("videos/top?limit={0}&offset={1}{2}{3}", limit, offset,
                String.Format("{0}", (game == null) ? "" : string.Format("&game={0}", game)),
                String.Format("{0}", (period == null) ? "" : string.Format("&period={0}", period))
                );

            return await HttpAction<TwitchVideoList_Response>(new Uri(BaseApiUri, uri_part), AUTHENTICATION.PUBLIC, REST_ACTION.GET);
        }

        // NOTE(duan): GET /channels/:channel/videos, call GetChannelVideos()

        // NOTE(duan): GET /videos/followed, call GetUserVideoFollows()

        #endregion

        #region [REST Functions ----------------------------------------------]
        /// NOTE(duan): Custom REST functions for navigating the TwitchAPI
        /// 
        /// 

        public enum REST_ACTION { DELETE, GET, POST, PUT, SEND }

        public enum AUTHENTICATION { PUBLIC, AUTHENTICATED }

        /// <summary>
        /// REST operation on a specific Uri and decodes the return string into a T Object.
        /// </summary>
        /// <typeparam name="T">Type of object to decode.</typeparam>
        /// <param name="uri">Uri path.</param>
        /// <param name="authentication">AUTHENTICATION mode.</param>
        /// <param name="action">REST_ACTION operation to perform.</param>
        /// <param name="data">Object to past to the REST operation.</param>
        /// <returns>T Object from the decoded return string.</returns>
        private async Task<T> HttpAction<T>(Uri uri, AUTHENTICATION authentication = AUTHENTICATION.PUBLIC, REST_ACTION action = REST_ACTION.GET, object data = null)
        {
            T ret = default(T);

            using (HttpClient hc = new HttpClient())
            {
                // add in public information
                hc.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv3+json");

                switch (authentication)
                {
                    // add in OAUTH token
                    case AUTHENTICATION.AUTHENTICATED:
                        // null check
                        if (this.OAuthToken == null)
                        {
                            return ret;
                        }

                        hc.DefaultRequestHeaders.Add("Authorization", "OAuth " + this.OAuthToken);
                        break;
                }

                HttpResponseMessage m = null;
                switch (action)
                {
                    case REST_ACTION.GET:
                        m = await hc.GetAsync(uri);

                        switch (m.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                // read content
                                string content = await m.Content.ReadAsStringAsync();
                                ret = GetDataFromContent<T>(ref content);
                                break;

                            case (HttpStatusCode)422:
                                // 422 Unprocessable Entity
                                // TODO(duan): log this
                                break;

                            default:
                                break;
                        }
                        break;

                    case REST_ACTION.PUT:
                        StringContent body = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data));
                        body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        m = await hc.PutAsync(uri, body);

                        switch (m.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                string content = await m.Content.ReadAsStringAsync();
                                ret = GetDataFromContent<T>(ref content);
                                break;
                            case (HttpStatusCode)422:
                                // 422 Unprocessable Entity
                                // TODO(duan): log this
                                break;
                        }
                        break;

                    case REST_ACTION.POST:
                        m = await hc.PostAsync(uri, (FormUrlEncodedContent)data);

                        switch (m.StatusCode)
                        {
                            case HttpStatusCode.OK:
                            case HttpStatusCode.NoContent:
                                // read content
                                string content = await m.Content.ReadAsStringAsync();
                                ret = GetDataFromContent<T>(ref content);
                                break;

                            case (HttpStatusCode)422: 
                                // 422 Unprocessable Entity
                                // TODO(duan): log this
                                // NOTE(duan): should pass through to the default case
                            default:
                                if (ret is bool)
                                {
                                    ret = (T)Convert.ChangeType(false, typeof(bool));
                                }
                                else
                                {
                                }
                                break;
                        }
                        break;

                    case REST_ACTION.DELETE:
                        m = await hc.DeleteAsync(uri);
                        switch (m.StatusCode)
                        {
                            case HttpStatusCode.NoContent:
                            case HttpStatusCode.OK:
                                // read content
                                string content = await m.Content.ReadAsStringAsync();
                                ret = GetDataFromContent<T>(ref content);
                                break;

                            case (HttpStatusCode)422:
                                // 422 Unprocessable Entity
                                // TODO(duan): log this
                                // NOTE(duan): should pass through to the default case
                            default:
                                if (ret is bool)
                                {
                                    ret = (T)Convert.ChangeType(false, typeof(bool));
                                }
                                break;
                        }
                        break;
                }
            }

            return ret;
        }

        /// <summary>
        /// Decode the object from the JSON content.
        /// </summary>
        /// <typeparam name="T">The Type T to convert the JSON to.</typeparam>
        /// <param name="content">The JSON content.</param>
        /// <returns>A Object of Type T from the JSON content on success, else default(T).</returns>
        private T GetDataFromContent<T>(ref string content)
        {
            T ret = default(T);
            try
            {
                if (ret is bool)
                {
                    ret = (T)Convert.ChangeType(true, typeof(bool));
                }
                else
                {
                    // decode content
                    ret = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
                }
            }
            catch (Exception ex)
            {
                // TODO(duan): log error
                string error_message = ex.Message;
            }
            return ret;
        }

        #endregion

        [Category("Twitch")]
        public string ChannelName { get; set; }

        private Uri _base_api_uri;
        [Category("Twitch API")]
        [DefaultValue(typeof(Uri), "https://api.twitch.tv/kraken/")]
        public Uri BaseApiUri
        {
            get { return _base_api_uri; }
            set
            {
                if (_base_api_uri != value)
                {
                    _base_api_uri = value;
                }
            }
        }

        private Uri _twitch_callback_uri;
        [Category("Twitch API")]
        [DefaultValue(typeof(Uri), "http://localhost")]
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

        private string _client_id;
        [Category("Twitch API")]
        public string ClientId
        {
            get { return _client_id; }
            set
            {
                if (_client_id != value)
                {
                    _client_id = value;
                }
            }
        }

        private string _oauth_token;
        [EditorBrowsable(EditorBrowsableState.Never)]
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

        private bool _log_errors;
        public bool LogErrors
        {
            get { return _log_errors; }
            set
            {
                if (_log_errors != value)
                {
                    _log_errors = value;
                }
            }
        }

        private TwitchScope _twitch_scope;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TwitchScope TwitchScope
        {
            get { return _twitch_scope; }
            set
            {
                if (_twitch_scope != value)
                {
                    _twitch_scope = value;
                }
            }
        }
    }

    /// <summary>
    ///  A custom attribute to allow a property to have a Category associated with it.
    /// </summary>
    public class Category : Attribute
    {
        public Category(string category)
        {
            _category_name = category;
        }

        private string _category_name;
        public string CategoryName
        {
            get { return _category_name; }
            set
            {
                if (_category_name != value)
                {
                    _category_name = value;
                }
            }
        }
    }
}

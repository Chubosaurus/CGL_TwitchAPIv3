using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGL.TwitchModels
{
    // NOTE(duan): taken from https://github.com/justintv/Twitch-API/blob/master/authentication.md

    // user_read: Read access to non-public user information, such as email address.
    // user_blocks_edit: Ability to ignore or unignore on behalf of a user.
    // user_blocks_read: Read access to a user's list of ignored users.
    // user_follows_edit: Access to manage a user's followed channels.
    // channel_read: Read access to non-public channel information, including email address and stream key.
    // channel_editor: Write access to channel metadata (game, status, etc).
    // channel_commercial: Access to trigger commercials on channel.
    // channel_stream: Ability to reset a channel's stream key.
    // channel_subscriptions: Read access to all subscribers to your channel.
    // user_subscriptions: Read access to subscriptions of a user.
    // channel_check_subscription: Read access to check if a user is subscribed to your channel.
    // chat_login: Ability to log into chat and send messages.
    // channel_feed_read: Ability to view to a channel feed.
    // channel_feed_edit: Ability to add posts and reactions to a channel feed.

    // NOTE(duan): we are going to do this manually rather then use the [Flags]
    public enum TwitchScope {
        none = 0,
        user_read = 1,
        user_blocks_edit = 2,
        user_blocks_read = 4,
        user_follows_edit = 8,
        channel_read = 16,
        channel_editor = 32,
        channel_commercial = 64,
        channel_stream = 128,
        channel_subscriptions = 256,
        user_subscriptions = 512,
        channel_check_subscription = 1024,
        chat_login = 2048,
        channel_feed_read = 4096,
        channel_feed_edit = 8192,
        full_control = 0x3FFF
    }
}

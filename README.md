
# CGL_TwitchAPIv3
An Asynchronous Portal Class Library for Twitch API version 3

>dotNET v4.5 (WINFORMS, WPF, etc...)<br>
>WINDOWS 8, WINDOWS 8.1, WINDOWS 10<br>
>WINDOWS PHONE 8 Silverlight<br>
>WINDOWS PHONE 8.1<br> 
>XBOX<br> 

<img src="http://i.imgur.com/SslrEi2.png" alt="PCL_TwitchAPIv3 Unit Test">


---

####Setup

1. Add the CGL_TwitchAPIv3 (Portable) project into your solution.
2. You can use as is...if you have an OAUTH token and ClientId from Twitch, else you need to create a new class that inherits from CGL_TwitchAPIv3 and override the public virtual string GetOAuthToken() to create your own custom form interface.
3. You can see an example of this in UT_WINFORMS project (WINFORMS Unit Test Project that is included in this repository).

---

####Usage

```C#
using CGL;
using CGL.TwitchModels;

TwitchAPIv3 TwitchAPI = TwitchAPIv3();
TwitchAPI.BaseApiUri = new Uri("https://api.twitch.tv/kraken/");
TwitchAPI.ClientId = "YOUR_CLIENT_ID";
TwitchAPI.TwitchCallbackUri = new Uri("http://localhost");
TwitchAPI.ChannelName = "YOUR_CHANNEL";
// NOTE: if you're not using the form, you can hardcode your OAuthToken here
//TwitchAPI.OAuthToken = "YOUR_TOKEN"

// Get the list of Top Games
TwitchGamesList_Response GET_TOP_GAMES = await TwitchAPI.GetTopGames();

// Get the list of Top Videos
TwitchVideoList_Response GET_TOP_VIDEOS = await TwitchAPI.GetTopVideos();
```

---

####License Information
CGL_TwitchAPIv3 is licensed under Attribution-NonCommercial-ShareAlike **CC BY-NC-SA**

<a href="https://creativecommons.org/licenses/by-nc-sa/4.0/"><img src="https://licensebuttons.net/l/by-nc-sa/3.0/88x31.png"></a>

---

####Other
CGL_TwitchAPIv3 uses <a href="http://www.newtonsoft.com/json">Json.NET - Newtonsoft</a> as its JSON library.






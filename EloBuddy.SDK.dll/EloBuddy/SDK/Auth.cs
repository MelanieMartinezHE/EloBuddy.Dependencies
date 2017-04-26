namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.Sandbox;
    using EloBuddy.SDK.Events;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Specialized;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    internal static class Auth
    {
        static Auth()
        {
            Loading.OnLoadingComplete += new Loading.LoadingCompleteHandler(Auth.Loading_OnLoadingComplete);
        }

        internal static void Initialize()
        {
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            try
            {
                MessageAuthInfo message = new MessageAuthInfo {
                    Username = SandboxConfig.Username,
                    PasswordHash = SandboxConfig.PasswordHash,
                    GameId = Game.GameId,
                    IsCustomGame = Game.IsCustomGame,
                    GameVersion = Game.Version,
                    Region = Game.Region,
                    SummonerName = "Joduskame",
                    Champion = EloBuddy.ObjectManager.Player.ChampionName,
                    HWID = SandboxConfig.Hwid
                };
                SendToServer<MessageAuthInfo>(message);
            }
            catch (Exception)
            {
            }
        }

        [PermissionSet(SecurityAction.Assert, Unrestricted=true)]
        internal static void SendToServer<T>(T message) where T: struct
        {
            string str = JsonConvert.SerializeObject(message);
            using (WebClient client = new WebClient())
            {
                client.Proxy = null;
                NameValueCollection data = new NameValueCollection {
                    [typeof(T).Name] = str
                };
                client.UploadValuesAsync(new Uri("https://edge.elobuddy.net/api.php?action=clientMessage"), data);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MessageAuthInfo
        {
            [DataMember]
            public string Username;
            [DataMember]
            public string PasswordHash;
            [DataMember]
            public ulong GameId;
            [DataMember]
            public bool IsCustomGame;
            [DataMember]
            public string GameVersion;
            [DataMember]
            public string Region;
            [DataMember]
            public string SummonerName;
            [DataMember]
            public string Champion;
            [DataMember]
            public string HWID;
        }
    }
}


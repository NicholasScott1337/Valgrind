using HarmonyLib;
using SmartBepInMods.Tools;
using SmartBepInMods.Tools.Patching.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Valgrind.Events
{
    class Server : Singleton<Server>
    {
        public static void SendMessageToAdmin(string desiredName, string text, bool tag = false, string avatar = null)
        {
            SmartBepInMods.Tools.Discord.Webhook.Execute(MyTokens.GetValue("AdminID", ""), MyTokens.GetValue("AdminKEY", ""), new SmartBepInMods.Tools.Discord.Webhook.ExecuteForm()
            {
                avatar_url = avatar ?? "https://i.ibb.co/9NWLhRC/external-content-duckduckgo.png",
                username = desiredName,
                content = $"{(tag ? "<@&816637234253791233> " : "")}{text}",
            });
        }
        public static void SendMessageToSupport(string desiredName, string text, bool tag = false, string avatar = null)
        {
            SmartBepInMods.Tools.Discord.Webhook.Execute(MyTokens.GetValue("SupportID", ""), MyTokens.GetValue("SupportKEY", ""), new SmartBepInMods.Tools.Discord.Webhook.ExecuteForm()
            {
                avatar_url = avatar ?? "https://i.ibb.co/9NWLhRC/external-content-duckduckgo.png",
                username = desiredName,
                content = $"{(tag ? "<@&816628174480998411> " : "")}{text}",
            });
        }
        public static void SendMessageToIngame(string desiredName, string text, string avatar = null)
        {
            SmartBepInMods.Tools.Discord.Webhook.Execute(MyTokens.GetValue("IngameID", ""), MyTokens.GetValue("IngameKEY", ""), new SmartBepInMods.Tools.Discord.Webhook.ExecuteForm()
            {
                avatar_url = avatar ?? "https://i.ibb.co/9NWLhRC/external-content-duckduckgo.png",
                username = desiredName,
                content = text, 
            });
        }

        private List<SpecialUsers.User> _SpecialUsers = new List<SpecialUsers.User>();

        public List<SpecialUsers.User> ExceptionalUsers
        {
            get
            {
                if (_SpecialUsers.Count != 0) return _SpecialUsers;
                _SpecialUsers = SmartBepInMods.Tools.HTTP.Json.DecodeUserDB<SpecialUsers.User>(
                    new SmartBepInMods.Tools.HTTP.HTTPRequest(
                        $"https://jsonbase.com/{MyTokens.GetValue("JsonID", "demo_bucket")}/{MyTokens.GetValue("JsonKEY", "")}",
                        SmartBepInMods.Tools.HTTP.HTTPRequest.HTTPMethod.GET
                        ).SendIt()).ToList();
                return _SpecialUsers;
            }
        }
        
        public static SmartBepInMods.Tools.Steam.SteamUser ValidateSteamUserInfo(string SteamID)
        {
            SmartBepInMods.Tools.Steam.SteamUser SteamProfile;

            if (KnownProfiles.Exists(f => f.steamid == SteamID))
            {
                SteamProfile = KnownProfiles.Find(f => f.steamid == SteamID);
            }
            else
            {
                KnownProfiles.Add(SmartBepInMods.Tools.Steam.SteamUser.GetFromID(SteamID, MyTokens.GetValue("SteamKEY", "")));
                SteamProfile = KnownProfiles[KnownProfiles.Count - 1];
            }
            return SteamProfile;
        }
        public static List<SmartBepInMods.Tools.Steam.SteamUser> KnownProfiles = new List<SmartBepInMods.Tools.Steam.SteamUser>();
        public struct SpecialUsers
        {
            public class User : SmartBepInMods.Tools.HTTP.Json.JsonReturn
            {
                public string steamid64;
                public string prefix;
                public string prefixColorHTML;
                public string nameColorHTML;
                public string textColorHTML;
                public bool staff;
                public bool vip;

                public void Apply(ref string username, ref string message)
                {
                    if (!this.vip) Clean(ref message);
                    username =  $"<color=#{this.prefixColorHTML}>{this.prefix}</color>" +
                        $"{(this.nameColorHTML != "" ? $" <color=#{this.nameColorHTML}>" : "")}{username}" +
                        $"{(this.nameColorHTML != "" ? " </color>" : "")}";
                    message = $"{(this.textColorHTML != "" ? $" <color=#{this.textColorHTML}>" : "")}{message}" +
                        $"{(this.textColorHTML != "" ? " </color>" : "")}";
                }
                public static void Clean(ref string toClean)
                {
                    toClean = Regex.Replace(toClean, "<.+?>", "");
                }

                public override void SetValue(string name, string value)
                {
                    switch (name)
                    {
                        case "prefix":
                            prefix = value;
                            break;
                        case "nameColorHTML":
                            nameColorHTML = value;
                            break;
                        case "prefixColorHTML":
                            prefixColorHTML = value;
                            break;
                        case "textColorHTML":
                            textColorHTML = value;
                            break;
                        case "vip":
                            vip = bool.Parse(value);
                            break;
                        case "staff":
                            staff = bool.Parse(value);
                            break;
                        case "steamid64":
                            steamid64 = value;
                            break;
                    }
                }
            }
        }
        public struct CHAT
        {
            public static bool Awake(Chat self)
            {
                ZRoutedRpc.instance.Register<string, int, Vector3, string>("StaffMessage", RPC.StaffMessage);
                return CONST.NOSKIP;
            }
            public static bool OnNewChatMessage(Chat self, long senderID, Talker.Type type, ref string user, ref string text)
            {

                return CONST.NOSKIP;
            }
        }
        public struct RPC
        {
            public static void StaffMessage(long shizid, string text, int type, Vector3 headpoint, string desiredName)
            {
                if (text == "I have arrived!") return;

                var Peer = ZNet.instance.GetPeer(shizid);
                var SteamID = Peer.m_socket.GetHostName(); // Very reliable and secure(heh)

                SmartBepInMods.Tools.Steam.SteamUser SteamProfile = ValidateSteamUserInfo(SteamID);


                if (Server.Instance.ExceptionalUsers.Exists(f => f.steamid64 == SteamID))
                {
                    // STAFF
                    var Member = Server.Instance.ExceptionalUsers.Find(f => f.steamid64 == SteamID);

                    if (text.Contains("#admin") && (Member.staff || Member.vip))
                    {
                        text = text.Replace("#admin", "");
                        SpecialUsers.User.Clean(ref text);
                        SendMessageToAdmin($"[{SteamID}]({SteamProfile.personaname}) {desiredName}", text, true, SteamProfile.avatarmedium);
                        return;
                    } 
                    else if (text.Contains("#support"))
                    {
                        if (Member.staff)
                        {
                            text = text.Replace("#support", "");
                            string x = "" + text;
                            SpecialUsers.User.Clean(ref x);
                            SendMessageToIngame("[SUPPORT]", x);
                            SendMessageToAdmin($"[{SteamID}]({SteamProfile.personaname}) {desiredName}", $"<@&817948367455518736> `Sent anonymous support message` \n{x}", false, SteamProfile.avatarmedium);
                            Member.Apply(ref desiredName, ref text);
                            ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, "ChatMessage", new object[] { 
                                headpoint,
                                2,
                                "<color=GOLD>[SUPPORT]</color>",
                                text
                            });
                            return;
                        }
                        else
                        {
                            text = text.Replace("#support", "");
                            SendMessageToSupport(desiredName, text, true);
                            return;
                        }
                    } 
                    else if (((Talker.Type)type) == Talker.Type.Shout)
                    {
                        var x = "" + text;
                        SpecialUsers.User.Clean(ref x);
                        SendMessageToIngame(desiredName, x);
                        SendMessageToAdmin($"[{SteamID}] ({SteamProfile.personaname}) {desiredName}", $"`Sent message via Shout channel` \n{x}", false, SteamProfile.avatarmedium);
                        Member.Apply(ref desiredName, ref text);
                        ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, "ChatMessage", new object[]
                        {
                            headpoint,
                            2,
                            desiredName,
                            text
                        });
                        return;
                    }
                    else
                    {
                        var x = "" + text;
                        SpecialUsers.User.Clean(ref x);
                        SendMessageToAdmin($"[{SteamID}] ({SteamProfile.personaname}) {desiredName}", $"`Sent message via {((Talker.Type)type == Talker.Type.Whisper ? "Whisper" : (Talker.Type)type == Talker.Type.Normal ? "Normal" : "Ping")} channel` \n{x}", false, SteamProfile.avatarmedium);
                        Member.Apply(ref desiredName, ref text);
                        ZRoutedRpc.instance.InvokeRoutedRPC(shizid, "RemoteSay", new object[] {
                            (int)type,
                            desiredName,
                            text
                        });
                        return;
                    }

                } else
                {
                    // Scrub their input
                    SpecialUsers.User.Clean(ref text);
                    SpecialUsers.User.Clean(ref desiredName);
                    if (text.Contains("#support"))
                    {
                        text.Replace("#support", "");
                        SendMessageToSupport(desiredName, text, true);
                    } else if (((Talker.Type)type) == Talker.Type.Shout)
                    {
                        SendMessageToAdmin($"[{SteamID}] ({SteamProfile.personaname}) {desiredName}", $"`Sent message via Shout channel` \n{text}", false, SteamProfile.avatarmedium);
                        SendMessageToIngame(desiredName, text);
                        ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, "ChatMessage", new object[]
                        {
                            headpoint,
                            (int) 2,
                            desiredName,
                            text
                        });
                    } else
                    {
                        SendMessageToAdmin($"[{SteamID}] ({SteamProfile.personaname}) {desiredName}", $"`Sent message via {((Talker.Type)type == Talker.Type.Whisper ? "Whisper" : (Talker.Type)type == Talker.Type.Normal ? "Normal" : "Ping")} channel` \n{text}", false, SteamProfile.avatarmedium);
                        ZRoutedRpc.instance.InvokeRoutedRPC(shizid, "RemoteSay", new object[] {
                            (int)type,
                            desiredName,
                            text
                        });
                    }
                }
            }
        }
        public struct ZNET
        {
            public static bool ServerHandshake(ZNet __instance, ref ZRpc rpc)
            {
                var peer = typeof(ZNet).GetMethod("GetPeer", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, new Type[]{
                    typeof(ZRpc)
                }, null).Invoke(__instance, new object[] {
                    rpc
                });
                if (peer == null)
                {
                    return SmartBepInMods.Tools.Patching.Constants.CONST.SKIP;
                }
                typeof(ZNet).GetMethod("ClearPlayerData", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
                {
                    typeof(ZNetPeer),
                }, null).Invoke(__instance, new object[] { peer });

                var SteamID = (peer as ZNetPeer).m_socket.GetHostName(); // Very reliable and secure(heh)

                SendMessageToAdmin("Valgrind Plus", $"User with SteamID: {SteamID} tried to join the server!", false);

                (peer as ZNetPeer).m_rpc.Invoke("Error", new object[] { 7 });
                (peer as ZNetPeer).m_rpc.Invoke("ValgrindHandshake", new object[] { 0 });

                return SmartBepInMods.Tools.Patching.Constants.CONST.SKIP;
            }
        }
    }
}

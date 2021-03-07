using SmartBepInMods.Tools;
using System;
using UnityEngine;
using UnityEngine.UI;
using SmartBepInMods.Tools.Patching.Constants;
using HarmonyLib;
using System.Collections.Generic;
using System.Net;

namespace Valgrind.Events
{
    class Client : Singleton<Client>
    {
        public static void RemoteSay(long id, int type, string user, string text)
        {
            AccessTools.FieldRefAccess<Talker, ZNetView>(Player.m_localPlayer.GetComponent<Talker>(), "m_nview").InvokeRPC(ZNetView.Everybody, "Say", new object[]
            {
                    type,
                    user,
                    text
            });
        }
        public struct CHAT
        {
            public static bool Awake(Chat self)
            {
                Graphic[] componentsInChildren = self.m_chatWindow.GetComponentsInChildren<Graphic>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].raycastTarget = false;
                }

                ZRoutedRpc.instance.Register<int, string, string>("RemoteSay", RemoteSay);
                return CONST.NOSKIP;
            }
            public static bool OnNewChatMessage(Chat self, ref float ___m_hideTimer, GameObject go, long senderID, Vector3 pos, Talker.Type type, ref string user, ref string text)
            {
                ___m_hideTimer = 0f;
                typeof(Chat).GetMethod("AddString", CONST.ALLFLAGS, null, new Type[] {
                    typeof(string), typeof(string), typeof(Talker.Type)
                }, null).Invoke(self, new object[] {
                    user, text, type
                });

                typeof(Chat).GetMethod("AddInworldText", CONST.ALLFLAGS).Invoke(self, new object[] {
                    go, senderID, pos, type, user, text
                });
                return CONST.SKIP;
            }
            public static bool SendText(Chat self, Talker.Type type, string text)
            {
                var Ply = Player.m_localPlayer;
                if (Ply)
                {
                    ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, "StaffMessage", new object[]
                    {
                    text,
                    (int) type,
                    Ply.GetHeadPoint(),
                    Ply.GetPlayerName()
                    });
                }
                return CONST.SKIP;
            }
        }
        public struct FEJDSTARTUP
        {
            public static Button JoinValgrind;

            static bool up = true;
            static double i = 0;
            public static bool Start(FejdStartup self) 
            {
                Button v = GameObject.Instantiate(self.m_manualIPButton, self.m_serverListPanel.transform);

                v.gameObject.transform.SetPositionAndRotation(new Vector3(v.gameObject.transform.position.x, v.gameObject.transform.position.y - 50, v.gameObject.transform.position.z), v.gameObject.transform.rotation);
                v.GetComponentInChildren<Text>().text = "[V+] Valgrind Server";
                v.onClick = new Button.ButtonClickedEvent();
                v.onClick.AddListener(() =>
                {
                    ZSteamMatchmaking.instance.QueueServerJoin("67.222.140.86:6956");
                });

                JoinValgrind = v;

                return CONST.NOSKIP;
            }
            public static bool Update(FejdStartup self)
            {
                if (JoinValgrind != null)
                {
                    var x = Time.deltaTime;

                    i = i + (0.1 * x * (up ? 1 : -1));
                    if (i >= 1) up = false;
                    if (i <= 0) up = true;

                    JoinValgrind.GetComponentInChildren<Text>().color = SmartBepInMods.Tools.HTTP.ColorExtensions.HSL2RGB(i, 0.5, 0.5);
                }
                return CONST.NOSKIP;
            }
        }
        public struct ZSTEAMMATCHMAKING
        {
            public static void RequestDedicatedServers()
            {
                var x = AccessTools.FieldRefAccess<ZSteamMatchmaking, List<ServerData>>(ZSteamMatchmaking.instance, "m_dedicatedServers");
                var y = new ServerData();
                var z = IPAddress.Parse("67.222.140.86").GetAddressBytes();
                var z2 = (uint)z[0] << 24;
                z2 += (uint)z[1] << 16;
                z2 += (uint)z[2] << 8;
                z2 += (uint)z[3];
                y.m_name = "<color=#ff>[V+]</color> Valgrind | Plugins | TP";
                y.m_steamHostAddr.SetIPv4(z2, 6956);
                y.m_password = true;
                y.m_players = 420;
                y.m_version = "Have Fun!";
                x.Add(y);
            }
        }
    }
}

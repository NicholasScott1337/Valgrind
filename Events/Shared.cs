using HarmonyLib;
using SmartBepInMods.Tools;
using System;
using UnityEngine;

namespace Valgrind.Events
{
    class Shared : Singleton<Shared>
    {
        public struct ZNET
        {
            public static void ValgrindHandshake(ZRpc rpc, int can, string id)
            {
                if (ZNet.instance.IsServer())
                {
                }
                else
                {
                    int myCan = 0;
                    foreach (var x in Plugin.FindObjectsOfType<GameObject>())
                    {
                        if (x.name == id)
                        {
                            myCan++;
                        }
                    }
                    rpc.Invoke("ValgrindHandshake", new object[] { myCan, "ErrorCount" });
                }
            }
            public static bool OnNewConnection(ZNet __instance, ref ZNetPeer peer)
            {
                peer.m_rpc.Register<int, string>("ValgrindHandshake", ZNET.ValgrindHandshake);
                return SmartBepInMods.Tools.Patching.Constants.CONST.NOSKIP;
            }
        }
    }
}

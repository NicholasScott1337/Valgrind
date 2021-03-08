using HarmonyLib;
using SmartBepInMods.Tools;
using System;
using UnityEngine;

namespace Valgrind.Events
{
    class Shared : Singleton<Shared>
    {
        public struct WEARNTEAR
        {
            public static bool Damage(WearNTear __instance, long sender, ref HitData hit)
            {
                if (AccessTools.FieldRefAccess<WearNTear, ZNetView>(__instance, "m_nview").IsOwner())
                {
                    if (AccessTools.FieldRefAccess<WearNTear, Piece>(__instance, "m_piece") && AccessTools.FieldRefAccess<WearNTear, Piece>(__instance, "m_piece").IsPlacedByPlayer())
                    {
                        if (!PrivateArea.CheckAccess((__instance as UnityEngine.Component).transform.position, 0, true))
                        {
                            return SmartBepInMods.Tools.Patching.Constants.CONST.SKIP;
                        }
                    }
                }
                return SmartBepInMods.Tools.Patching.Constants.CONST.NOSKIP;
            }
        }
        public struct ZNET
        {
            public static void ValgrindHandshake(ZRpc rpc, int can)
            {
                if (ZNet.instance.IsServer())
                {
                }
                else
                {
                    int myCan = 0;
                    foreach (var x in Plugin.FindObjectsOfType<GameObject>())
                    {
                        if (x.name == "SkToolbox")
                        {
                            myCan = 1;
                        }
                    }
                    rpc.Invoke("ValgrindHandshake", new object[] { myCan });
                }
            }
            public static bool OnNewConnection(ZNet __instance, ref ZNetPeer peer)
            {
                peer.m_rpc.Register<int>("ValgrindHandshake", ZNET.ValgrindHandshake);
                return SmartBepInMods.Tools.Patching.Constants.CONST.NOSKIP;
            }
        }
    }
}

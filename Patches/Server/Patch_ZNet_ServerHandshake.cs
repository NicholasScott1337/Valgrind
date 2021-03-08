using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Server
{
    [HarmonyPatch(typeof(ZNet), "RPC_ServerHandshake", null)]
    class Patch_ZNet_ServerHandshake : SmartBepInMods.Tools.Patching.Constants.SERVER
    {
        private static bool Prefix(ZNet __instance, ref ZRpc rpc)
        {
            return Events.Server.ZNET.ServerHandshake(__instance, ref rpc);
        }
    }
}

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Shared
{
    [HarmonyPatch(typeof(ZNet), "OnNewConnection", null)]
    class Patch_ZNet_OnNewConnection : SmartBepInMods.Tools.Patching.Constants.SHARED
    {
        private static void Prefix(ZNet __instance, ref ZNetPeer peer)
        {
            Events.Shared.ZNET.OnNewConnection(__instance, ref peer);
        }
    }
}

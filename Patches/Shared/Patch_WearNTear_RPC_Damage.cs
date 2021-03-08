using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Shared
{
    [HarmonyPatch(typeof(WearNTear), "RPC_Damage", null)]
    class Patch_WearNTear_RPC_Damage : SmartBepInMods.Tools.Patching.Constants.SHARED
    {
        private static bool Prefix(WearNTear __instance, long sender, ref HitData hit)
        {
            return Events.Shared.WEARNTEAR.Damage(__instance, sender, ref hit);
        }
    }
}

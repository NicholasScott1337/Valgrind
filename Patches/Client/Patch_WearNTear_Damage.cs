using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Client
{
    [HarmonyPatch(typeof(WearNTear), "Damage", null)]
    class Patch_WearNTear_Damage : SmartBepInMods.Tools.Patching.Constants.CLIENT
    {
        private static bool Prefix(WearNTear __instance, ref HitData hit)
        {
            return Events.Client.WEARNTEAR.Damage(__instance, ref hit);
        }
    }
}

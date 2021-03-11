using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Client
{
    [HarmonyPatch(typeof(Destructible), "Damage", null)]
    class Patch_Destructible_Damage : SmartBepInMods.Tools.Patching.Constants.CLIENT
    {
        private static bool Prefix(Destructible __instance, ref HitData hit)
        {
            return Events.Client.DESTRUCTIBLE.Damage(__instance, ref hit);
        }
    }
}

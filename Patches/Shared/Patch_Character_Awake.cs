using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Shared
{
    [HarmonyPatch(typeof(Character), "Awake", null)]
    class Patch_Character_Awake : SmartBepInMods.Tools.Patching.Constants.SHARED
    {
        private static void Postfix(Character __instance)
        {
            ZNetView znetView = AccessTools.Field(typeof(Character), "m_nview").GetValue(__instance) as ZNetView;
            if (znetView.GetZDO() != null && !__instance.IsPlayer() && (!znetView.HasOwner() || znetView.IsOwner()) && __instance.GetHealth() == __instance.GetMaxHealth())
            {
                AccessTools.Method(typeof(Character), "SetupMaxHealth", null, null).Invoke(__instance, null);
            }
        }
    }
}

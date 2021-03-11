using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Shared
{
    [HarmonyPatch(typeof(Pickable), "Awake", null)]
    class Patch_Pickable_Awake : SmartBepInMods.Tools.Patching.Constants.SHARED
    {
        private static void Postfix(Pickable __instance)
        {
            Plugin.LOG.LogInfo($"Patching Pickable {__instance.m_itemPrefab.name} from {__instance.m_respawnTimeMinutes} to {__instance.m_respawnTimeMinutes / 4}");
            __instance.m_respawnTimeMinutes /= 4;
        }
    }
}

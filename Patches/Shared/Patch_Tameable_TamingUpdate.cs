using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Shared
{
    [HarmonyPatch(typeof(Tameable), "TamingUpdate", null)]
    class Patch_Tameable_TamingUpdate : SmartBepInMods.Tools.Patching.Constants.SHARED
    {
        private static void Postfix(Tameable __instance)
        {
            if ((int)AccessTools.Method(typeof(Tameable), "GetTameness", null, null).Invoke(__instance, null) > 0)
            {
                (AccessTools.Field(typeof(Tameable), "m_monsterAI").GetValue(__instance) as MonsterAI).SetDespawnInDay(false);
            }
        }
    }
}

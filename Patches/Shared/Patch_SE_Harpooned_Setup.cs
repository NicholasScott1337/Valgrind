using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Shared
{
    [HarmonyPatch(typeof(SE_Harpooned), "Setup", null)]
    class Patch_SE_Harpooned_Setup : SmartBepInMods.Tools.Patching.Constants.SHARED
    {
        private static void Postfix(SE_Harpooned __instance)
        {
            __instance.m_minDistance = 2f;
        }
    }
}

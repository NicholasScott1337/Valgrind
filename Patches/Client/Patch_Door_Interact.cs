using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Client
{
    [HarmonyPatch(typeof(Door), "Interact", null)]
    class Patch_Door_Interact : SmartBepInMods.Tools.Patching.Constants.CLIENT
    {
        private static void Prefix(Door __instance, ZNetView ___m_nview)
        {
            Events.Client.DOOR.Interact(__instance, ___m_nview);
        }
    }
}

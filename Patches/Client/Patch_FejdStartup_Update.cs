using HarmonyLib;
using SmartBepInMods.Tools.Patching;
using SmartBepInMods.Tools.Patching.Constants;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Valgrind.Patches.Client
{
    /// <summary>
    /// Rainbow effect on the Join V+ Server button.
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), "Update", null)]
    public class Patch_FejdStartup_Update : CLIENT
    {
        private static void Postfix(FejdStartup __instance)
        {
            Events.Client.FEJDSTARTUP.Update(__instance);
        }
    }
}

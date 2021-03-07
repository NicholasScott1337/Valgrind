using HarmonyLib;
using SmartBepInMods.Tools.Patching.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace Valgrind.Patches.Client
{
    /// <summary>
    /// New button for joining the server
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), "Start", null)]
    public class Patch_FejdStartup_Start : CLIENT
    {
        private static void Postfix(FejdStartup __instance)
        {
            Events.Client.FEJDSTARTUP.Start(__instance);
        }
    }
}

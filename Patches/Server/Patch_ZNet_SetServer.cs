using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SmartBepInMods.Tools.Patching.Constants;

namespace Valgrind.Patches.Server
{
    [HarmonyPatch(typeof(ZNet), "SetServer", null)]
    class Patch_ZNet_SetServer : SERVER
    {
        private static void Prefix(ZNet __instance, ref string password, ref string serverName)
        {
            password = "";
            serverName = "<color=RED>[V+]</color> <color=CYAN><i>Valgrind</i></color>";
        }
    }
}

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Client
{
    [HarmonyPatch(typeof(ZNet), "RPC_Error", null)]
    class Patch_ZNet_Error : SmartBepInMods.Tools.Patching.Constants.CLIENT
    {
        private static bool Prefix(ZNet __instance, int error)
        {
            if (error == 7) return SmartBepInMods.Tools.Patching.Constants.CONST.SKIP;
            return SmartBepInMods.Tools.Patching.Constants.CONST.NOSKIP;
        }
    }
}

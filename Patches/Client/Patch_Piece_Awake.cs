using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Client
{
    [HarmonyPatch(typeof(Piece), "Awake", null)]
    class Patch_Piece_Awake : SmartBepInMods.Tools.Patching.Constants.CLIENT
    {
        private static void Postfix(Piece __instance)
        {
            __instance.m_allowedInDungeons = true;
        }
    }
}

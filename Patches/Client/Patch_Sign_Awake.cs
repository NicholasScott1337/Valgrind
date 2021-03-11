using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Client
{
    [HarmonyPatch(typeof(Sign), "Awake", null)]
    class Patch_Sign_Awake : SmartBepInMods.Tools.Patching.Constants.CLIENT
    {
        private static void Prefix(Sign __instance)
        {
            __instance.m_textWidget.supportRichText = true;
            __instance.m_textWidget.color = Color.white;
            __instance.m_textWidget.material = null;
            __instance.m_characterLimit = 999;
        }
    }
}

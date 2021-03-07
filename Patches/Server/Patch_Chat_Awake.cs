﻿using HarmonyLib;
using SmartBepInMods.Tools.Patching.Constants;

namespace Valgrind.Patches.Server
{
    [HarmonyPatch(typeof(Chat), "Awake")]
    class Patch_Chat_Awake : SERVER
    {
        // Token: 0x06000004 RID: 4 RVA: 0x0000210C File Offset: 0x0000030C
        private static void Postfix(Chat __instance)
        {
            Events.Server.CHAT.Awake(__instance);
        }
    }
}

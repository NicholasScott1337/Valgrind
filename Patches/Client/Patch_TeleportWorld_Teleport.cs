using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Client
{
    [HarmonyPatch(typeof(TeleportWorld), "Teleport", null)]
    class Patch_TeleportWorld_Teleport : SmartBepInMods.Tools.Patching.Constants.CLIENT
    {
        private static void Prefix(TeleportWorld __instance)
        {
            Events.Client.DOOR.CloseTrackedDoor();
        }
    }
}

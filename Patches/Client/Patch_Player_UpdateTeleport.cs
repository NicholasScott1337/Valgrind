using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Client
{
    [HarmonyPatch(typeof(Player), "UpdateTeleport", null)]
    class Patch_Player_UpdateTeleport : SmartBepInMods.Tools.Patching.Constants.CLIENT
    {
        private static void Prefix(ref bool ___m_teleporting, Player __instance, ref float ___m_teleportTimer, ref float dt)
        {
            dt *= 3f;
            if (___m_teleportTimer > 0f && ___m_teleportTimer <= 2f)
            {
                ___m_teleportTimer = 2f;
            }
            if (___m_teleporting)
            {
                Hud.instance.m_loadingScreen.alpha = 1f;
            }
        }
    }
}

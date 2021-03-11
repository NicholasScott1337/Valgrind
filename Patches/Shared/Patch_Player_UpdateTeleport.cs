using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Shared
{
    [HarmonyPatch(typeof(Player), "UpdateTeleport", null)]
    class Patch_Player_UpdateTeleport : SmartBepInMods.Tools.Patching.Constants.SHARED
    {
        private static void Postfix(Player __instance, ref bool ___m_teleporting, ref float ___m_teleportTimer, ref Vector3 ___m_teleportTargetPos, ref Quaternion ___m_teleportTargetRot)
        {
			if (___m_teleporting && (double)___m_teleportTimer > 2.0)
			{
				foreach (Character character in Patch_Player_TeleportTo.pets)
				{
					Vector2 vector = new Vector2(1f, 0f) + UnityEngine.Random.insideUnitCircle;
					Vector3 b = new Vector3(vector.x, vector.y, 0.5f);
					Vector3 lookDir = ___m_teleportTargetRot * Vector3.forward;
					character.transform.position = ___m_teleportTargetPos + b;
					character.transform.rotation = ___m_teleportTargetRot;
					character.SetLookDir(lookDir);
				}
			}
		}
    }
}

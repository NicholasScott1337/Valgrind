using SmartBepInMods.Tools.Transpiling;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Reflection.Emit;

namespace Valgrind.Patches.Shared
{
    [HarmonyPatch(typeof(SE_Harpooned), "UpdateStatusEffect", null)]
    class Patch_SE_Harpooned_UpdateStatusEffect : SmartBepInMods.Tools.Patching.Constants.SHARED
    {
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.SmartNopAll(new KeyValuePair<CodeInstruction, Qualifier>[]
            {
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Ldarg_0), Qualifier.Default),
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Ldfld), Qualifier.Default),
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Callvirt), Qualifier.Default),
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Ldnull), Qualifier.Default),
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Call), Qualifier.Default),
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Brfalse_S), Qualifier.Default),
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Ldarg_0), Qualifier.Default),
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Ldfld), Qualifier.Default),
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Callvirt), Qualifier.Default),
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Brtrue_S), Qualifier.Default),
            });
        }
        [HarmonyPrefix]
        private static void Prefix(SE_Harpooned __instance, float dt, Character ___m_attacker)
        {
            if (___m_attacker.GetZDOID() == Player.m_localPlayer.GetZDOID())
            {
                var x = Input.GetAxis("Mouse Wheel");
                if (x != 0)
                {
                    __instance.m_minDistance += x * (5 * dt);
                    __instance.m_maxDistance += x * (5 * dt);
                    var y = x * (5 * dt);
                    __instance.m_minDistance = Mathf.Clamp(__instance.m_minDistance + y, 1f, 20f);
                    __instance.m_maxDistance = Mathf.Clamp(__instance.m_maxDistance + y, 30f, 50f);
                    MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, $"Harpoon Distance changed to {Math.Round(__instance.m_minDistance, 1)}");
                }
            }
        }
    }
}

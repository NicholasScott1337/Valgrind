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
    }
}

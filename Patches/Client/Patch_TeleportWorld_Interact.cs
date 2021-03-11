using HarmonyLib;
using System;
using System.Collections.Generic;
using SmartBepInMods.Tools.Transpiling;
using System.Reflection.Emit;

namespace Valgrind.Patches.Client
{
    [HarmonyPatch(typeof(TeleportWorld), "Interact", null)]
    class Patch_TeleportWorld_Interact : SmartBepInMods.Tools.Patching.Constants.CLIENT
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.SmartMatch(new KeyValuePair<CodeInstruction, Qualifier>[] {
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Call), Qualifier.OpCode),
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Ldarg_0), Qualifier.OpCode),
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Ldstr), Qualifier.OpCode),
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Ldc_I4_S), Qualifier.OpCode),
                new KeyValuePair<CodeInstruction, Qualifier>(new CodeInstruction(OpCodes.Callvirt), Qualifier.OpCode),
            }, MatchFound);
        }
        public static IEnumerable<CodeInstruction> MatchFound(IEnumerable<CodeInstruction> Source)
        {
            foreach (var x in Source)
            {
                if (x.opcode == OpCodes.Ldc_I4_S)
                {
                    x.operand = 999;
                    yield return x;
                } else
                {
                    yield return x;
                }
            }
        }
    }
}

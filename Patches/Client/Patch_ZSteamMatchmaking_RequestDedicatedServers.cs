using HarmonyLib;
using SmartBepInMods.Tools.Transpiling;
using SmartBepInMods.Tools.Patching.Constants;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Emit;

namespace Valgrind.Patches.Client
{
    /// <summary>
    /// Server priority. Top of the list with 420 babayy.
    /// </summary>
    [HarmonyPatch(typeof(ZSteamMatchmaking), "RequestDedicatedServers", null)]
    public class Patch_ZSteamMatchmaking_RequestDedicatedServers : CLIENT
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.SmartPostfix(new Dictionary<CodeInstruction, Qualifier>()
            {
                { new CodeInstruction(OpCodes.Ldarg_0), Qualifier.Default },
                { new CodeInstruction(OpCodes.Ldfld), Qualifier.Default },
                { new CodeInstruction(OpCodes.Brfalse_S), Qualifier.Default },
                { new CodeInstruction(OpCodes.Ldarg_0), Qualifier.Default },
                { new CodeInstruction(OpCodes.Ldfld), Qualifier.Default },
                { new CodeInstruction(OpCodes.Call), Qualifier.Default },
                { new CodeInstruction(OpCodes.Ldarg_0), Qualifier.Default },
                { new CodeInstruction(OpCodes.Ldc_I4_0), Qualifier.Default },
                { new CodeInstruction(OpCodes.Stfld), Qualifier.Default },
                { new CodeInstruction(OpCodes.Ldarg_0), Qualifier.Default },
                { new CodeInstruction(OpCodes.Ldfld), Qualifier.Default },
                { new CodeInstruction(OpCodes.Callvirt), Qualifier.Default },
            }, new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Call, typeof(Events.Client.ZSTEAMMATCHMAKING).GetMethod(nameof(Events.Client.ZSTEAMMATCHMAKING.RequestDedicatedServers)))
            });
        }
    }
}

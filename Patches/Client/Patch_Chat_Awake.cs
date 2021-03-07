using HarmonyLib;
using SmartBepInMods.Tools.Patching.Constants;

namespace Valgrind.Patches.Client
{
    // Client
    [HarmonyPatch(typeof(Chat), "Awake")]
    class Patch_Chat_Awake : CLIENT
    {
        // Token: 0x06000004 RID: 4 RVA: 0x0000210C File Offset: 0x0000030C
        private static void Postfix(Chat __instance)
        {
            Valgrind.Events.Client.CHAT.Awake(__instance);
        }
    }
}

using HarmonyLib;
using SmartBepInMods.Tools.Patching.Constants;

namespace Valgrind.Patches.Client
{
    [HarmonyPatch(typeof(Chat), "SendText", null)]
    class Patch_Chat_SendText : CLIENT
    {
        private static bool Prefix(Chat __instance, Talker.Type type, string text)
        {
            return Events.Client.CHAT.SendText(__instance, type, text);
        }
    }
}

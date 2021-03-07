using HarmonyLib;
using SmartBepInMods.Tools.Patching.Constants;
using System.Text.RegularExpressions;

namespace Valgrind.Patches.Server
{
    // Server
    [HarmonyPatch(typeof(Chat), "OnNewChatMessage", null)]
    class Patch_Chat_OnNewChatMessage : SERVER
    {
        public static bool Prefix(Chat __instance, long senderID, Talker.Type type, ref string user, ref string text)
        {
            return Events.Server.CHAT.OnNewChatMessage(__instance, senderID, type, ref user, ref text);
        }
    }
}

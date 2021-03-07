using HarmonyLib;
using System;
using SmartBepInMods.Tools.Patching.Constants;
using UnityEngine;
using System.Reflection;

namespace Valgrind.Patches.Client
{
    // Client
    [HarmonyPatch(typeof(Chat), "OnNewChatMessage", null)]
    class Patch_Chat_OnNewChatMessage : CLIENT
    {
        private static bool Prefix(Chat __instance, ref float ___m_hideTimer, GameObject go, long senderID, Vector3 pos, Talker.Type type, ref string user, ref string text)
        {
            Valgrind.Events.Client.CHAT.OnNewChatMessage(__instance, ref ___m_hideTimer, go, senderID, pos, type, ref user, ref text);
            return false;
        }
    }
}

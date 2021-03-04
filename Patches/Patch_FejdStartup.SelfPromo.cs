using HarmonyLib;
using SmartBepInMods.Transpiling;
using SmartBepInMods.Patching;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Valgrind.Util;
using SmartBepInMods.Patching.Constants;

namespace Valgrind.Patches
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
                new CodeInstruction(OpCodes.Call, typeof(Patch_ZSteamMatchmaking_RequestDedicatedServers).GetMethod(nameof(ServerPost)))
            }, Valgrind.Plugin.LOG.LogInfo);
        }

        public static void ServerPost()
        {
            var x = AccessTools.FieldRefAccess<ZSteamMatchmaking, List<ServerData>>(ZSteamMatchmaking.instance, "m_dedicatedServers");
            var y = new ServerData();
            var z = IPAddress.Parse("67.222.140.86").GetAddressBytes();
            var z2 = (uint)z[0] << 24;
            z2 += (uint)z[1] << 16;
            z2 += (uint)z[2] << 8;
            z2 += (uint)z[3];
            y.m_name = "<color=#ff>[V+]</color> Valgrind | Plugins | TP";
            y.m_steamHostAddr.SetIPv4(z2, 6956);
            y.m_password = true;
            y.m_players = 420;
            y.m_version = "Have Fun!";
            x.Add(y);

        }
    }
    /// <summary>
    /// New button for joining the server
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), "Start", null)]
    public class Patch_FejdStartup_Start : CLIENT
    {
        public static Button JoinValgrind;
        private static void Postfix(FejdStartup __instance)
        {
            Button v = GameObject.Instantiate(__instance.m_manualIPButton, __instance.m_serverListPanel.transform);

            v.gameObject.transform.SetPositionAndRotation(new Vector3(v.gameObject.transform.position.x, v.gameObject.transform.position.y - 50, v.gameObject.transform.position.z), v.gameObject.transform.rotation);
            v.GetComponentInChildren<Text>().text = "[V+] Valgrind Server";
            v.onClick = new Button.ButtonClickedEvent();
            v.onClick.AddListener(() =>
            {
                ZSteamMatchmaking.instance.QueueServerJoin("67.222.140.86:6956");
            });

            JoinValgrind = v;
        }
    }
    /// <summary>
    /// Rainbow effect on the Join V+ Server button.
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), "Update", null)]
    public class Patch_FejdStartup_Update : CLIENT
    {
        static bool up = true;
        static double i = 0;
        private static void Postfix(FejdStartup __instance)
        {
            if (Patch_FejdStartup_Start.JoinValgrind != null)
            {
                var x = Time.deltaTime;

                i = i + (0.1 * x * (up ? 1 : -1));
                if (i >= 1) up = false;
                if (i <= 0) up = true;

                Patch_FejdStartup_Start.JoinValgrind.GetComponentInChildren<Text>().color = ClassExtensions.HSL2RGB(i, 0.5, 0.5);
            }
        }
    }
}

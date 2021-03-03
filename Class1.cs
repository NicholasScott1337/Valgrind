using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Valgrind
{
    [BepInPlugin("com.nicholascott.valheim.valgrindclient", "Valgrind", "0.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LOG;
        public void Awake()
        {
            LOG = this.Logger;
            var x = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly()).GetPatchedMethods().ToArray().Length;
            base.Logger.LogInfo($"Patched {x} method{(x > 1 ? "s" : "")}");
        }
    }
}

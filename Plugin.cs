using BepInEx;
using BepInEx.Logging;
using SmartBepInMods.Tools.Patching;
using SmartBepInMods.Tools.Patching.Constants;
using SmartBepInMods.Tools.Logging;
using System.Reflection;
using UnityEngine;

namespace Valgrind
{
    [BepInPlugin("com.nicholascott.valheim.valgrindclient", "Valgrind", "0.1.1")]
    public class Plugin : BaseUnityPlugin
    {


        public static ManualLogSource LOG;
        private static Assembly _Assembly;
        public void Awake()
        {
            LOG = this.Logger;
            _Assembly = Assembly.GetExecutingAssembly();

            SmartLog.Instance.LogChannel += (s) => { LOG.LogInfo(s.obj); };

            _Assembly.PatchGameAuto(_DEBUG);
        }

        public void Update()
        {
            if (_Assembly.GetEnvArg() == typeof(SERVER))
            {
                // Do server stuff
            }
            else if (_Assembly.GetEnvArg() == typeof(CLIENT))
            {
                // Do client stuff
            }
        }
    }
}

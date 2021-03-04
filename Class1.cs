using BepInEx;
using BepInEx.Logging;
using SmartBepInMods.Tools.Patching;
using SmartBepInMods.Tools.Patching.Constants;
using System.Reflection;

namespace Valgrind
{
    [BepInPlugin("com.nicholascott.valheim.valgrindclient", "Valgrind", "0.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LOG;
        private static Assembly _Assembly;
        public void Awake()
        {
            LOG = this.Logger;
            _Assembly = Assembly.GetExecutingAssembly();

            _Assembly.PatchGameAuto(LOG.LogInfo, false);
        }

        public void Update()
        {
            if (_Assembly.GetEnvArg(LOG.LogInfo) == typeof(SERVER))
            {
                // Do server stuff
            }
            else if (_Assembly.GetEnvArg(LOG.LogInfo) == typeof(CLIENT))
            {
                // Do client stuff
            }
        }
    }
}

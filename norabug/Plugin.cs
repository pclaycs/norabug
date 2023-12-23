using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using norabug.Patches;

namespace norabug
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }

        public static ManualLogSource Log { get; private set; }

        private void Awake()
        {
            //IL_0016: Unknown result type (might be due to invalid IL or missing references)
            //IL_001b: Unknown result type (might be due to invalid IL or missing references)
            //IL_002b: Unknown result type (might be due to invalid IL or missing references)
            Instance = this;
            Log = ((Plugin)this).Logger;

            // Plugin startup logic
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Log.LogInfo("LMFAOOOOOOOOO PAULY'S IN");

            var harmony = new Harmony("pissbucket1.norabug");
            harmony.PatchAll(typeof(HoarderBugNameplatePatch));
        }
    }
}

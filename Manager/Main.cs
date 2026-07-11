using BepInEx;
using Core.Tools;
using HarmonyLib;

namespace Manager {
    [BepInPlugin(pluginId, Core.Static.Game + "Manager", Core.Static.VersionStr)]
    public class Main : BaseUnityPlugin {
        private const string pluginId = Core.Static.PluginIdRoot + ".manager";
        private readonly Harmony harmony = new Harmony(pluginId);

        public void Awake() {
            harmony.PatchAll();

            Core.Static.OnConfigParsed += UseConfig;
            Core.Tools.Config.Parse();

            Core.Static.OnBellTriggered += () => Core.Tools.Config.Parse();
        }

        private void UseConfig(Config config) {
            if (config is null) return;

            Data.Time.TimeScale = config.CutsceneSpeed.Value;
            Data.Time.SpeedOnCredits = config.EnableSpeedOnCredits.Value;
        }
    }
}

using BepInEx;
using Core.Tools;
using HarmonyLib;

namespace Core {
    [BepInPlugin(pluginId, Static.Game + "Core", Static.VersionStr)]
    public class Main : BaseUnityPlugin {
        private const string pluginId = Static.PluginIdRoot + ".core";
        public void Awake() {
            var harmony = new Harmony(pluginId);
            harmony.PatchAll();

            Static.OnConfigParsed += UseConfig;
            Tools.Config.Parse();

            Static.OnBellTriggered += () => Tools.Config.Parse();
        }

        private void UseConfig(Config config) {
            if (config is null) return;

            Data.Time.TimeScale = config.CutsceneSpeed.Value;
            Data.Time.UnlockScale = config.UnlockAnimationSpeed.Value;
        }
    }
}

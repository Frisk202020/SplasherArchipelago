using BepInEx;
using HarmonyLib;
using SplasherArchipelago.Public;

namespace SplasherManager {
    [BepInPlugin(pluginId, "SplasherManager", "0.0.6")]
    public class Main : BaseUnityPlugin {
        private const string pluginId = "com.frisk.splashermanager";
        private readonly Harmony harmony = new Harmony(pluginId);

        public void Awake() {
            harmony.PatchAll();
            var config = SplasherArchipelago.Shared.Parse();
            if (config is null) {
                SplasherArchipelago.Shared.OnConfigParsed += (parsedConfig) => UseConfig(parsedConfig);
                return;
            }

            UseConfig(config);

        }

        private void UseConfig(Config config) {
            if (config is null) return;

            Data.Time.TimeScale = config.CutsceneSpeed.Value;
            if (!config.EnableSpeedOnCredits.Value) {
                SplasherArchipelago.Shared.CreditsEvent += () => Patches.TimeManager.Disable();
            }
        }
    }
}

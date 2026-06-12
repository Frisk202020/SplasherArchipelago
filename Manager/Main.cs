using BepInEx;
using HarmonyLib;

namespace SplasherManager {
    [BepInPlugin(pluginId, "SplasherManager", "0.0.5")]
    public class Main : BaseUnityPlugin {
        private const string pluginId = "com.frisk.splashermanager";
        private readonly Harmony harmony = new Harmony(pluginId);

        public void Awake() {
            harmony.PatchAll();
            var config = SplasherArchipelago.Shared.Parse();
            if (config is null) return;

            var scale = config.CutsceneSpeed;
            Data.Time.TimeScale = scale is null ? 1 : scale.Value;
        }
    }
}

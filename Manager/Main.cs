using BepInEx;
using HarmonyLib;

namespace SplasherManager {
    [BepInPlugin(pluginId, "SplasherManager", "0.0.4")]
    public class Main : BaseUnityPlugin {
        private const string pluginId = "com.frisk.splashermanager";
        private readonly Harmony harmony = new Harmony(pluginId);

        public void Awake() {
            harmony.PatchAll();
        }
    }
}

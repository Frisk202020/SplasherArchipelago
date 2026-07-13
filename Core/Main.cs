using BepInEx;
using HarmonyLib;

namespace Core {
    [BepInPlugin(pluginId, Static.Game + "Core", Static.VersionStr)]
    public class Main : BaseUnityPlugin {
        private const string pluginId = Static.PluginIdRoot + ".core";
        public void Awake() {
            Static.Log("Sta");
            var harmony = new Harmony(pluginId);
            harmony.PatchAll();
        }
    }
}

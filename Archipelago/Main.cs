using BepInEx;

namespace Archipelago {
    [BepInPlugin(Util.PluginId, Core.Static.Game + "Archipelago", Core.Static.VersionStr)]
    public class Main : BaseUnityPlugin {
        public void Awake() {}

        public void OnDestroy() {
            Network.Helpers.ProxyManager.Drop();
        }
    }
}
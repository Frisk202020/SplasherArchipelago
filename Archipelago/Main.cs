using BepInEx;

namespace SplasherArchipelago {
    [BepInPlugin(Util.pluginId, "SplasherArchipelago", "0.0.3")]
    public class Main : BaseUnityPlugin {
        public void Awake() {}

        public void OnDestroy() {
            Network.Helpers.ProxyManager.Drop();
        }
    }
}
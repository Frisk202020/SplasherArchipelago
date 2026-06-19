using BepInEx;

namespace SplasherArchipelago {
    [BepInPlugin(Shared.pluginId, "SplasherArchipelago", "0.0.6")]
    public class Main : BaseUnityPlugin {
        public void Awake() {}

        public void OnDestroy() {
            Network.Helpers.ProxyManager.Drop();
        }
    }
}
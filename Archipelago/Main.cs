using BepInEx;
using System;

namespace SplasherArchipelago {
    [BepInPlugin(Shared.pluginId, "SplasherArchipelago", "0.0.5")]
    public class Main : BaseUnityPlugin {
        public void Awake() {}

        public void OnDestroy() {
            Network.Helpers.ProxyManager.Drop();
        }
    }
}
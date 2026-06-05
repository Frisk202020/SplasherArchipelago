using BepInEx;
using System;

namespace SplasherArchipelago {
    [BepInPlugin(Util.pluginId, "SplasherArchipelago", "0.0.4")]
    public class Main : BaseUnityPlugin {
        public void Awake() {}

        public void OnDestroy() {
            Network.Helpers.ProxyManager.Drop();
        }
    }
}
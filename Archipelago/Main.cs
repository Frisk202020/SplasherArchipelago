using BepInEx;

namespace Archipelago {
    [BepInPlugin(Util.PluginId, Core.Static.Game + "Archipelago", Core.Static.VersionStr)]
    public class Main : BaseUnityPlugin {
        public void Awake() {
            Core.Static.OnBellTriggered += () => {
                if (Core.Tools.Config.Parse())
                    Network.ArchipelagoManager.Start(Core.Tools.Config.Instance);
            };
        }

        public void OnDestroy() {
            Network.Helpers.ProxyManager.Drop();
        }
    }
}
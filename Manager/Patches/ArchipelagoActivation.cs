using HarmonyLib;
namespace SplasherManager.Patches {
    [HarmonyPatch(typeof(HubBell), "OnTriggerEnter")]
    public static class ArchipelagoActivation {
        private static bool enabled = false;

        public static bool Prefix() {
            if (enabled) {
                return true;
            }
            
            enabled = SplasherArchipelago.Shared.Start();
            if (enabled) {
                GameData.Initialized = false;
                GameData.Instance.InitializePlayerData();
                Hub.Load();
            }

            return false;

        }
    }
}

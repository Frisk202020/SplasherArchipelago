using HarmonyLib;
namespace SplasherManager.Patches {
    [HarmonyPatch(typeof(HubBell), "OnTriggerEnter")]
    public static class ArchipelagoActivation {
        private static bool enabled = false;

        public static bool Prefix() {
            if (enabled) {
                return true;
            }
            
            enabled = SplasherArchipelago.Util.Start();
            if (enabled) Hub.Load();

            return false;

        }
    }
}

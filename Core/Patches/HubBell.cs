using HarmonyLib;

namespace Core.Patches {
    [HarmonyPatch(typeof(HubBell), "OnTriggerEnter")]
    public static class ArchipelagoActivation {
        public static bool Prefix() {
            Static.StartBellEvents();
            return true;
        }
    }
}

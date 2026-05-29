using HarmonyLib;
using SplasherArchipelago.Network;

namespace SplasherArchipelago.Patches.Controller {
    [HarmonyPatch(typeof(HubBell), "OnTriggerEnter")]
    public static class Bell {
        public static bool Prefix() {
            ArchipelagoManager.Reconnect();
            return true;
        }
    }
}

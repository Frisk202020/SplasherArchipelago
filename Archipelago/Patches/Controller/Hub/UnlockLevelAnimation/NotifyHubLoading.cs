using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller.Hub.UnlockLevelAnimation {
    [HarmonyPatch(typeof(global::Hub), "Load")]
    public static class NotifyHubLoading {
        public static void Postfix() {
            Data.HubState.DoorsLoaded = false;
        }
    }
}

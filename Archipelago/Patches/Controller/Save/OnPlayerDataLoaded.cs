using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller.Save {
    [HarmonyPatch(typeof(GameData), "RefreshLevelData")]
    public static class OnPlayerDataLoaded {
        public static void Postfix(GameData __instance) {
            if (__instance.CurrentPlayerData != null && !Network.ArchipelagoManager.SaveLoaded) Network.ArchipelagoManager.FinalizeSaveLoading();
        }
    }
}

using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller.Save {
    [HarmonyPatch(typeof(GameData), "CurrentPlayerData", MethodType.Setter)]
    public static class OnPlayerDataLoaded {
        public static void Postfix(PlayerSaveData value) {
            if (value != null && !Network.ArchipelagoManager.SaveLoaded) Network.ArchipelagoManager.FinalizeSaveLoading();
        }
    }
}

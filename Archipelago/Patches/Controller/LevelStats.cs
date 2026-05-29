using HarmonyLib;

namespace SplasherArchipelago.Patches.Setup {
    [HarmonyPatch(typeof(GameData), "GetLevelData")]
    public static class LevelStats {
        private static void Patch(LevelData data, int index) {
            data.State = Data.Items.LevelKeys.IsLevelUnlocked(index)
                ? Data.Locations.LocationOnEachLevel.Clears.IsCleared(index)
                    ? HubDoorState.Finished
                    : HubDoorState.Unlocked
                : HubDoorState.Locked;

            data.ActualRescuedSplashers = Data.Locations.Splashers.RescuedForLevel(
                GameData.Instance.LevelMetaDataList[index].LevelName
            );

            return;
        }


        public static bool Prefix(GameData __instance, string sceneName) {
            var index = __instance.LevelMetaDataList.IndexOf(__instance.GetLevelMetaData(sceneName));
            var data = __instance.CurrentPlayerData.GetLevelData(index);
            Patch(data, index);
        
            return true;
        }
    }
}

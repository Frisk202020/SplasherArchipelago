using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller {
    [HarmonyPatch(typeof(PlayerSaveData), "GetLevelData")]
    public static class LevelStats {
        public static bool Prefix(PlayerSaveData __instance, int levelIndex) {
            __instance.LevelDataList[levelIndex].State = Data.Items.LevelKeys.IsLevelUnlocked(levelIndex) 
                ? Data.Locations.LocationOnEachLevel.Clears.IsCleared(levelIndex)
                    ? HubDoorState.Finished
                    : HubDoorState.Unlocked
                : HubDoorState.Locked;

            __instance.LevelDataList[levelIndex].ActualRescuedSplashers = Data.Locations.Splashers.RescuedForLevel(
                GameData.Instance.LevelMetaDataList[levelIndex].LevelName
            );
 
            return true;    
        }
    }
}
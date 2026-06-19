using HarmonyLib;

/**
 * Set the actual unlock state of a level based of Archipelago data.
 * If configuration requires to show level names, doors are marked as finished if unlocked.
 */

namespace SplasherArchipelago.Patches.Controller.Hub {
    [HarmonyPatch(typeof(GameData), "GetLevelData")]
    public static class Door {
        internal static bool ShowName = false;

        private static void Patch(LevelData data, int index) {
            data.State = index == 0 || Data.Items.LevelKeys.IsLevelUnlocked(index - 1)
                ? ShowName || Data.Locations.LocationOnEachLevel.Clears.IsCleared(index)
                    ? HubDoorState.Finished
                    : HubDoorState.Unlocked
                : HubDoorState.Locked;

            data.ActualRescuedSplashers = Data.Locations.Splashers.RescuedForLevel(
                GameData.Instance.LevelMetaDataList[index].LevelName
            );

            return;
        }


        public static bool Prefix(GameData __instance, string sceneName) {
            if (GameManager.Mode > GameMode.TimeAttack) return true;

            var index = __instance.LevelMetaDataList.IndexOf(__instance.GetLevelMetaData(sceneName));
            var data = __instance.CurrentPlayerData.GetLevelData(index);
            Patch(data, index);

            return true;
        }
    }
}

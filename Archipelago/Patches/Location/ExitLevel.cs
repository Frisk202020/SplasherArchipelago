using HarmonyLib;
using Archipelago.Helpers;

/**
 * Detect a level clear location
 */

namespace Archipelago.Patches.Location {
    [HarmonyPatch(typeof(Exit), "CoroutineEndLevel")]
    public static class ExitLevel {
        public static bool Prefix() {
            var name = GameData.Instance.CurrentLevelMetaData.LevelName;
            Data.Locations.Clears.Check(GameData.Instance.CurrentLevelData, (int)LevelByName.Id(name));

            if (GameManager.Mode != GameMode.Standard) return true;

            for (int i = 0; i < VictimeManager.SplasherCount; i++) {
                var splasher = VictimeManager.Instance.splashers[i];
                if (splasher.Rescued || splasher.AlreadySaved) {
                    Data.Locations.Splashers.Check(name, i);
                }
            }

            return true;
        }
    }
}

using HarmonyLib;

namespace SplasherArchipelago.Patches.Location {
    [HarmonyPatch(typeof(Exit), "ExitLevel")]
    public static class ExitLevel {
        public static bool Prefix() {
            var name = GameData.Instance.CurrentLevelMetaData.LevelName;

            if (!Data.Locations.LocationOnEachLevel.Clears.IsCleared(GameData.Instance.CurrentLevelMetaData.LevelName)) {
                Data.Locations.LocationOnEachLevel.Clears.Clear(name);
            }

            var rescued = GameData.Instance.CurrentLevelData.RescuedSplashers;
            for (int i = 0; i < rescued.Length; i++) {
                if (rescued[i]) {
                    Data.Locations.Splashers.Rescue(name, i);
                }
            }

            return true;
        }
    }
}

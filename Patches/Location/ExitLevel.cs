using HarmonyLib;

namespace SplasherArchipelago.Patches.Location {
    [HarmonyPatch(typeof(Exit), "ExitLevel")]
    public static class ExitLevel {
        public static bool Prefix() {
            if (!Data.Locations.LocationOnEachLevel.Clears.IsCleared(GameData.Instance.CurrentLevelMetaData.LevelName)) {
                var name = GameData.Instance.CurrentLevelMetaData.LevelName;
                Data.Locations.LocationOnEachLevel.Clears.Clear(name);
            }

            return true;
        }
    }
}

using HarmonyLib;

namespace SplasherArchipelago.Patches.Location {
    [HarmonyPatch(typeof(UIScorePanel), "Refresh")]
    public static class Speedrun {
        public static bool Prefix(ref bool showWorld) {
            showWorld = false;
            return true;
        }

        public static void Postfix(LevelMetaData lmd, Medal __result) {
            System.Console.WriteLine(__result);
            switch (__result) {
                case Medal.Bronze: Data.Locations.LocationOnEachLevel.Bronzes.Clear(lmd.LevelName); break;
                case Medal.Silver: Data.Locations.LocationOnEachLevel.Silvers.Clear(lmd.LevelName); goto case Medal.Bronze;
                case Medal.Gold: Data.Locations.LocationOnEachLevel.Golds.Clear(lmd.LevelName); goto case Medal.Silver;
                case Medal.Dev: Data.Locations.LocationOnEachLevel.Platinums.Clear(lmd.LevelName); goto case Medal.Gold;
            }
        }
    }
}

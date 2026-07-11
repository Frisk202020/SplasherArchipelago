using Archipelago.Helpers;

namespace Archipelago.Data.Locations {
    static class Splashers {
        private const int splashers_per_level = 7;

        internal static void Check(LocalizedString level, int splasherId) {
            var levelId = (int)LevelByName.Id(level);
            if (MarkRescued(levelId, splasherId))
                Network.ArchipelagoManager.Check(LocationType.Splasher, splashers_per_level * levelId + splasherId);
        }

        private static bool MarkRescued(int levelId, int splasherId) {
            var data = GameData.Instance.CurrentPlayerData.LevelDataList[levelId];
            if (data.ActualRescuedSplashers[splasherId]) return false;

            data.ActualRescuedSplashers[splasherId] = true;
            GameData.Instance.SavePlayerData();

            return true;
        }

        internal static void Restore(int id) {
            var splasherLocationId = id - (int)LocationType.Splasher;

            var levelId = splasherLocationId / splashers_per_level;
            var splasherId = splasherLocationId % splashers_per_level;

            MarkRescued(levelId, splasherId);
        }
    }
}

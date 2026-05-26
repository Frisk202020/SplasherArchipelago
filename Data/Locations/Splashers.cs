using SplasherArchipelago.Helpers;
using System.Collections.Generic;

namespace SplasherArchipelago.Data.Locations {
    static class Splashers {
        private const int splashers_per_level = 7;

        private static Dictionary<LocalizedString, bool[]> collected = EachLevel<bool[]>.Init(() => new bool[splashers_per_level]);

        private static bool guard(LocalizedString level, int splasherId) {
            return collected.ContainsKey(level) && splasherId >= 0 && splasherId < splashers_per_level;
        }

        internal static bool IsRescued(LocalizedString level, int splasherId) {
            if (!guard(level, splasherId)) return false;
            return collected[level][splasherId];
        }

        internal static bool[] RescuedForLevel(LocalizedString level) {
            if (!collected.ContainsKey(level)) return new bool[splashers_per_level];
            return collected[level];
        }

        internal static void Rescue(LocalizedString level, int splasherId) {
            if (!guard(level, splasherId)) return;

            collected[level][splasherId] = true;
            Network.ArchipelagoManager.Check(LocationType.Splasher, splashers_per_level * LevelByName.Id(level) + splasherId);
        }

        internal static void Check(int id) {
            var level = GameData.Instance.LevelMetaDataList[id / splashers_per_level].LevelName;
            collected[level][id % splashers_per_level] = true;
        }
    }
}

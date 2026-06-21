using SplasherArchipelago.Helpers;
using System.Collections.Generic;

namespace SplasherArchipelago.Data.Locations {
    internal static class Speedrun {
        private static readonly Dictionary<LocalizedString, bool> bronzes = EachLevel<bool>.Init(() => false);
        private static readonly Dictionary<LocalizedString, bool> silvers = EachLevel<bool>.Init(() => false);
        private static readonly Dictionary<LocalizedString, bool> golds = EachLevel<bool>.Init(() => false);
        private static readonly Dictionary<LocalizedString, bool> platinums = EachLevel<bool>.Init(() => false);

        private static Dictionary<LocationType, Dictionary<LocalizedString, bool>> byLocationType = new Dictionary<LocationType, Dictionary<LocalizedString, bool>> {
            { LocationType.Bronze, bronzes },
            { LocationType.Silver, silvers },
            { LocationType.Gold, golds },
            { LocationType.Platinum, platinums }
        };

        internal static Medal? GetMatchingMedal(LocalizedString level) {
            if (string.IsNullOrEmpty(level.GetString())) return null;

            foreach (var medal in new Medal[] {Medal.Dev, Medal.Gold, Medal.Silver, Medal.Bronze}) {
                if (byLocationType[medal.ToLocation().Value][level]) return medal;
            }

            return Medal.None;
        }

        private static void TryMutate(
            LocationType loc, 
            LocalizedString level, 
            bool sendCheck
         ) {
            var dict = byLocationType[loc];
            if (dict[level]) return;

            dict[level] = true;

            if (sendCheck) 
                Network.ArchipelagoManager.Check(loc, LevelByName.Id(level));
        }

        private static void UpdateScores(LocationType? loc, LocalizedString level, bool sendCheck) {
            switch(loc) {
                case LocationType.Platinum: TryMutate(LocationType.Platinum, level, sendCheck); goto case LocationType.Gold;
                case LocationType.Gold: TryMutate(LocationType.Gold, level, sendCheck); goto case LocationType.Silver;
                case LocationType.Silver: TryMutate(LocationType.Silver, level, sendCheck); goto case LocationType.Bronze;
                case LocationType.Bronze: TryMutate(LocationType.Bronze, level, sendCheck); break;
            }
        }

        internal static void Check(Medal medal, LocalizedString level) {
            UpdateScores(medal.ToLocation(), level, true);
        }

        internal static void Restore(LocationType loc, int id) {
            UpdateScores(loc, GameData.Instance.LevelMetaDataList[id - (int)loc].LevelName, false);
        }
    }
}

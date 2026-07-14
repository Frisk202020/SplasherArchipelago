using Archipelago.Helpers;
using System.Collections.Generic;

namespace Archipelago.Data.Locations {
    internal static class Speedrun {
        internal static Medal HighestRequiredMedal { get; private set; } = Medal.None;
        private static LocationType HighestRequiredLocation = 0;
        internal static void SetHighestMedal(Medal medal) {
            HighestRequiredMedal = medal;
            HighestRequiredLocation = medal.ToLocation() ?? 0;
        }

        private static readonly Dictionary<LocalizedString, bool> bronzes = EachLevel<bool>.Init(() => false);
        private static readonly Dictionary<LocalizedString, bool> silvers = EachLevel<bool>.Init(() => false);
        private static readonly Dictionary<LocalizedString, bool> golds = EachLevel<bool>.Init(() => false);
        private static readonly Dictionary<LocalizedString, bool> platinums = EachLevel<bool>.Init(() => false);

        private static readonly Dictionary<LocationType, Dictionary<LocalizedString, bool>> byLocationType = new Dictionary<LocationType, Dictionary<LocalizedString, bool>> {
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
            LocalizedString level
         ) {
            if (loc > HighestRequiredLocation) return;   

            var dict = byLocationType[loc];
            if (dict[level]) return;

            dict[level] = true;
            Network.ArchipelagoManager.Check(loc, LevelByName.Id(level));
        }

        private static void UpdateScores(LocationType? loc, LocalizedString level) {
            switch(loc) {
                case LocationType.Platinum: TryMutate(LocationType.Platinum, level); goto case LocationType.Gold;
                case LocationType.Gold: TryMutate(LocationType.Gold, level); goto case LocationType.Silver;
                case LocationType.Silver: TryMutate(LocationType.Silver, level); goto case LocationType.Bronze;
                case LocationType.Bronze: TryMutate(LocationType.Bronze, level); break;
            }
        }

        internal static void Check(Medal medal, LocalizedString level) {
            UpdateScores(medal.ToLocation(), level);
        }

        internal static void Restore(LocationType loc, int id) {
            UpdateScores(loc, GameData.Instance.LevelMetaDataList[id - (int)loc].LevelName);
        }
    }
}

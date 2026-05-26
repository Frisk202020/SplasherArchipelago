using SplasherArchipelago.Helpers;
using System.Collections.Generic;

namespace SplasherArchipelago.Data.Locations {
    class LocationOnEachLevel {
        public static readonly LocationOnEachLevel Clears = new LocationOnEachLevel();
        public static readonly LocationOnEachLevel Bronzes = new LocationOnEachLevel();
        public static readonly LocationOnEachLevel Silvers = new LocationOnEachLevel();
        public static readonly LocationOnEachLevel Golds = new LocationOnEachLevel();

        private LocationOnEachLevel() { }

        private readonly Dictionary<LocalizedString, bool> clears = EachLevel<bool>.Init(()=>false);

        internal void Clear(LocalizedString level) {
            if (!clears.ContainsKey(level)) return;

            clears[level] = true;
            Network.ArchipelagoManager.Check(LocationType.Clear, LevelByName.Id(level));
        }

        internal void Check(int id) {
            var level = GameData.Instance.LevelMetaDataList[id].LevelName;
            clears[level] = true;
        }

        internal bool IsCleared(LocalizedString level) {
            return clears.ContainsKey(level) ? clears[level] : false;
        }

        internal bool IsCleared(int index) {
            return IsCleared(GameData.Instance.LevelMetaDataList[index].LevelName);
        }
    }
}

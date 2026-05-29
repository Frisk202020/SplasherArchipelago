using System.Collections.Generic;

namespace SplasherArchipelago.Helpers {
    static class LevelByName {
        private static Dictionary<LocalizedString, uint> Init() {
            var data = new Dictionary<LocalizedString, uint>();
            for (uint i = 0; i < GameData.Instance.LevelMetaDataList.Count; i++) {
                data.Add(GameData.Instance.LevelMetaDataList[(int)i].LevelName, i);
            }

            return data;
        }
        private static readonly Dictionary<LocalizedString, uint> levelByName = Init();
        internal static uint Id(LocalizedString name) {
            return levelByName[name];
        }
    }
}

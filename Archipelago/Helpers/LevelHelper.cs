using System.Collections.Generic;

namespace Archipelago.Helpers {
    static class LevelByName {
        private static Dictionary<LocalizedString, int> Init() {
            var data = new Dictionary<LocalizedString, int>();
            for (int i = 0; i < Util.LevelCount; i++) {
                var level = GameData.Instance.LevelMetaDataList[i];
                data.Add(level.LevelName, i);
                sceneNames[i] = level.SceneName;
            }

            return data;
        }

        private static readonly string[] sceneNames = new string[Util.LevelCount];
        private static readonly Dictionary<LocalizedString, int> levelByName = Init();

        internal static int Id(LocalizedString name) {
            return levelByName[name];
        }

        internal static string Scene(int index) => sceneNames[index];
    }
}

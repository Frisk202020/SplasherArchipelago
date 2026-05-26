using System;
using System.Collections.Generic;

namespace SplasherArchipelago.Helpers {
    static class EachLevel<T> {
        internal static Dictionary<LocalizedString, T> Init(Func<T> get_default) {
            var data = new Dictionary<LocalizedString, T>();
            foreach(var level in GameData.Instance.LevelMetaDataList) {
                data.Add(level.LevelName, get_default());
            }

            return data;
        }
    }
}

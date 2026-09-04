using System;
using System.Collections.Generic;
using System.Linq;

namespace Archipelago.Data {
    class CheckpointTable {
        private class TableData<T> where T : class {
            private readonly Dictionary<string, T> table;
            internal TableData(Func<string, T> mapping) => table = Util.Scenes.ToDictionary(s => s, s => mapping(s));

            internal T Get(string scene=null) {
                var key = scene ?? GameData.Instance?.CurrentLevelMetaData?.SceneName;
                return key == null ? null : table[key];
            }
        } 

        private static readonly TableData<List<int>> dataTable = new TableData<List<int>>(
            s => {
                switch(s) {
                    case "A1": return Enumerable.Range(2, 3).ToList();
                    case "A6": return new List<int> { 2, 3, 5, 6, 7 };
                    case "A_Boss": return Enumerable.Range(2, 3).ToList();
                    case "B_Boss": return Enumerable.Range(2, 4).ToList();
                    case "C_Boss": return Enumerable.Range(2, 7).ToList();
                    default: return Enumerable.Range(2, 5).ToList();
                }
            }
        );
        private static int ParseId(string name) {
            var n = name.Contains("(1)") ? 5 : 1;
            return name[name.Length - n] - '0';
        }
        private static string NameFromId(int id, bool parenthesis) => $"LD_Checkpoint{id}{(parenthesis ? " (1)" : "")}";

        internal static int LocationId(string name) {
            var i = 0;
            var id = ParseId(name);
            var scene = GameData.Instance?.CurrentLevelMetaData.SceneName;

            foreach (var s in Util.Scenes) {
                var x = dataTable.Get(s);
                if (s == scene) return i + x.IndexOf(id);
                i += x.Count;
            }

            Core.Static.Warn($"Failed to find location id for scene {scene} : {name}");
            return -1;
        }
        internal static int GetRange(int sceneId) => dataTable.Get(Util.Scenes[sceneId]).Count;

        internal static string Next(string name, string scene=null) {
            var data = dataTable.Get(scene);
            if (data == null) return null;

            var i = data.IndexOf(ParseId(name));
            return (i == -1 || i >= (data.Count-1)) ? null : NameFromId(data[i + 1], false);
        }

        private readonly TableData<List<bool>> table;
        internal CheckpointTable() => table = new TableData<List<bool>>(s => Enumerable.Repeat(false, dataTable.Get(s).Count).ToList());

        internal bool Get(string name, string scene=null) {
            var t = table.Get(scene);
            if (t == null) return false;

            var id = dataTable.Get(scene).IndexOf(ParseId(name));
            return t[id];
        }

        internal void CheckLevel(string scene) {
            var t = table.Get(scene);
            if (t == null) return;

            for(var i = 0; i < t.Count; i++) { t[i] = true; }
        }
        internal void Check(string name) {
            var t = table.Get();
            if (t == null) return;

            var id = dataTable.Get().IndexOf(ParseId(name));
            t[id] = true;
        }
        internal void Check(int id, string scene) {
            table.Get(scene)[id] = true;
        }
        internal void CheckAll(string scene) {
            var t = table.Get(scene);
            for(var i = 0; i < t.Count; i++) { t[i] = true; }
        }
    }
}
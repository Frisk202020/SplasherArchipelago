using System.Collections.Generic;

namespace Archipelago.Data {
    class CheckpointTable {
        private static string SceneOrCurrent(string scene) => scene ?? GameData.Instance.CurrentLevelMetaData.SceneName;
        private static readonly Dictionary<string, int> specificCkpBounds = new Dictionary<string, int> {
            {"A1", 3},
            {"A_Boss", 3},
            {"B_Boss", 4},
            {"C1", 4},
            {"C_Boss", 7}
        };
        internal static int IdRange(string scene) {
            if (specificCkpBounds.ContainsKey(scene)) return specificCkpBounds[scene];
            return 5;
        }

        private static readonly HashSet<string> withParenthesis = new HashSet<string> { "A2", "A3" };
        internal static string NameById(int id, string scene=null) {
            var x = $"LD_Checkpoint{id + 2}";
            if (withParenthesis.Contains(SceneOrCurrent(scene))) x += " (1)";
            return x;
        }

        private static Dictionary<string, Dictionary<string, int>> idTable;
        internal static int Id(string name, string scene=null) {
            if (idTable == null) {
                idTable = new Dictionary<string, Dictionary<string, int>>();
                int id = 0;

                foreach(var s in Util.Scenes) {
                    var d = new Dictionary<string, int>();
                    for (int i = 0; i < IdRange(s); i++) {
                        d.Add(NameById(i, scene), id);
                        id++;
                    }

                    idTable.Add(s, d);
                }
            }

            return idTable[SceneOrCurrent(scene)][name];
        }

        private readonly Dictionary<string, Dictionary<string, bool>> table;

        internal CheckpointTable() {
            table = new Dictionary<string, Dictionary<string, bool>>();

            foreach(var scene in Util.Scenes) {
                var d = new Dictionary<string, bool>();
                for (int i = 0; i < IdRange(scene); i++) {
                    d.Add(NameById(i, scene), false);
                }

                table.Add(scene, d);
            }
        }

        internal bool Get(string name, string scene=null) => table[SceneOrCurrent(scene)][name];
        internal void Check(string name, string scene=null) => table[SceneOrCurrent(scene)][name] = true;
    }
}
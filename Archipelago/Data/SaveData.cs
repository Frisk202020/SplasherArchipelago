using HarmonyLib;
using System.Reflection;
using TSKGames.Save;

namespace Archipelago.Data {
    internal static class SaveData {
        internal static bool EnableTimeAttackDoors = false;
        private static Helpers.Save.SaveDataExtension data = new Helpers.Save.SaveDataExtension();
        internal static Helpers.Save.GameSaver Saver => new Helpers.Save.GameSaver(data);

        private static readonly MethodInfo DoorSetter = AccessTools.DeclaredPropertySetter(typeof(Door), "State");

        internal static HubDoorState GetDoorState(Door door, bool speedrun) {
            return EnableTimeAttackDoors && speedrun 
                ? data.TimeAttackState[Helpers.LevelByName.Id(door.levelMetaData.LevelName)]
                : GameData.Instance.GetLevelData(door.levelMetaData.SceneName).State;
        }

        internal static HubDoorState GetDoorState(int id, bool speedrun) {
            return EnableTimeAttackDoors && speedrun
                ? data.TimeAttackState[id]
                : GameData.Instance.GetLevelData(Helpers.LevelByName.Scene(id)).State;
        }

        internal static void SetDoorState(Door door, HubDoorState state, bool speedrun, bool setOnData) {
            DoorSetter.Invoke(door, new object[] { state });
            if (!setOnData) return;

            if (EnableTimeAttackDoors && speedrun) {
                data.TimeAttackState[Helpers.LevelByName.Id(door.levelMetaData.LevelName)] = state;
                return;
            }

            GameData.Instance.GetLevelData(door.levelMetaData.SceneName).State = state;
        }

        internal static void Init() {
            DataStore.OnAutosaveLoad += (save, savename) => {
                if (savename != Util.SaveFileExtension()) return;

                var s = save.Read<Helpers.Save.GameSaver>();
                if (s is null) {
                    Core.Static.Warn("Save not found. Using a new one...");
                    return;
                }

                data = s.data;
                Core.Static.Log("Archipelago Save Loaded");
            };

            DataStore.AutoSaveExist(Util.SaveFileExtension(), delegate (string filename, bool exist) {
                if (exist) {
                    DataStore.LoadAutoSave(Util.SaveFileExtension());
                    return;
                }
                Core.Static.Warn("Archipelago Save Not Found -- Expecting if starting a new Multiworld");
            });
        }
    }
}

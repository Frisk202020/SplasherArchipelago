using HarmonyLib;
using System.Reflection;
using TSKGames.Save;

namespace SplasherArchipelago.Data {
    internal static class SaveData {
        internal static bool EnableTimeAttackDoors = false;
        private static Helpers.Save.SaveDataExtension data = new Helpers.Save.SaveDataExtension();
        internal static Helpers.Save.GameSaver Saver => new Helpers.Save.GameSaver(data);

        private static MethodInfo DoorSetter = AccessTools.DeclaredPropertySetter(typeof(Door), "State");

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

        internal static void SetDoorState(Door door, HubDoorState state, bool speedrun, bool setOnDoorInstance) {
            if (setOnDoorInstance) DoorSetter.Invoke(door, new object[] { state });

            if (EnableTimeAttackDoors && speedrun) {
                data.TimeAttackState[Helpers.LevelByName.Id(door.levelMetaData.LevelName)] = state;
                return;
            }

            GameData.Instance.GetLevelData(door.levelMetaData.SceneName).State = state;
        }

        internal static void Init() {
            DataStore.OnAutosaveLoad += (save, savename) => {
                if (savename != Shared.SaveFileExtension()) return;

                var s = save.Read<Helpers.Save.GameSaver>();
                if (s is null) {
                    Util.Warn("Save not found. Using a new one...");
                    return;
                }

                data = s.data;
                Util.Log("Archipelago Save Loaded");
            };

            DataStore.AutoSaveExist(Shared.SaveFileExtension(), delegate (string filename, bool exist) {
                if (exist) {
                    DataStore.LoadAutoSave(Shared.SaveFileExtension());
                    return;
                }
                Util.Warn("Archipelago Save Not Found");
            });
        }
    }
}

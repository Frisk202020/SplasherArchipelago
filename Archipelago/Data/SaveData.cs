using TSKGames.Save;

namespace SplasherArchipelago.Data {
    internal static class SaveData {
        internal static Helpers.Save.SaveDataExtension data = new Helpers.Save.SaveDataExtension();
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

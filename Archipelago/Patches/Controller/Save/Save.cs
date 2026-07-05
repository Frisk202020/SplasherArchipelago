using HarmonyLib;
using TSKGames.Save;

/**
 * Use a save dedicated to the current Archipelago seed instead of the default one.
 */

namespace SplasherArchipelago.Patches.Controller.Save {
    [HarmonyPatch(typeof(DataStore), "AutoSaveSilently")]
    public static class Save {
        public static bool Prefix(ref string AutoSaveFilename) {
            if (AutoSaveFilename != Shared.VANILLA_FILE) return true;

            AutoSaveFilename = Shared.SaveFile();
            return true;
        }

        public static void Postfix(string AutoSaveFilename) {
            if (AutoSaveFilename == Shared.SaveFileExtension()) return;

            DataStore.AutoSaveSilently(
                Data.SaveData.Saver,
                Shared.SaveFileExtension()
            );

            Util.Log("Archipelago saved");
        }
    }
}
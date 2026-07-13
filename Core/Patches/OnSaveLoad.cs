using HarmonyLib;

namespace Core.Patches {
    [HarmonyPatch(typeof(GameData), "DataStore_OnAutosaveLoad")]
    public static class OnSaveLoad {
        public static bool Prefix(string savename) {
            return !Static.DataStoreBlacklist.Contains(savename);
        }
    }
}

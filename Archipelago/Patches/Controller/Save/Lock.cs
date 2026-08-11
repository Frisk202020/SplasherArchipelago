using HarmonyLib;

namespace Archipelago.Patches.Controller.Save {
    [HarmonyPatch(typeof(GameData), "SavePlayerData")]
    public static class Lock {
        private static bool locked = false;
        private static bool saveAgain = false;

        public static bool Prefix() {
            if (locked) {
                saveAgain = true;
                return false;
            }

            saveAgain = false;
            locked = true;
            return true;
        }

        public static void Postfix(GameData __instance) {
            locked = false;
            if (saveAgain) __instance.SavePlayerData();
        }
    }
}
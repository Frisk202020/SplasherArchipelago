using HarmonyLib;

namespace Archipelago.Patches.Controller {
    [HarmonyPatch(typeof(GameAchievements), "UnlockAchievement")]
    public static class Achievements {
        public static bool Prefix() { return false; }
    }
}

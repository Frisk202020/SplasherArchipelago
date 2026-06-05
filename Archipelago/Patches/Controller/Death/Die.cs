using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller {
    [HarmonyPatch(typeof(PlayerController), "Die")]
    public static class Die {

        public static bool Prefix() {
            Data.DeathLink.AddDeath();

            return true;
        }
    }
}

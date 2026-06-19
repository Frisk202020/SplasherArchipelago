using HarmonyLib;

/**
 * This patch allows to increment the death counter when the player dies.
 * This is useful for deathlink. The logic of wether the death should actually increase the counter is managed in data.
 */

namespace SplasherArchipelago.Patches.Controller {
    [HarmonyPatch(typeof(PlayerController), "Die")]
    public static class Die {

        public static bool Prefix() {
            Data.DeathLink.AddDeath();

            return true;
        }
    }
}

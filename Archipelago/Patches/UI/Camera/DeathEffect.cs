using HarmonyLib;

namespace Archipelago.Patches.UI.Camera {
    [HarmonyPatch(typeof(PlayerCamera), "PlayDeathEffect")]
    public static class DeathEffect {
        public static bool Prefix(PlayerCamera __instance) {
            if (Data.Poison.Poisoned()) {
                __instance.PlayEffect("Poisoned", 0);
                return false;
            }

            return true;
        }
    }
}
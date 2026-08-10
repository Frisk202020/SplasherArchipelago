using HarmonyLib;

/**
 * Kill the player if possible (because of deathlink or splasher death in hero mode).
 */

namespace Archipelago.Patches.Controller.Death {
    [HarmonyPatch(typeof(PlayerController), "State", MethodType.Getter)]
    public static class PlayerState {
        public static void Postfix(PlayerController __instance) {
            if (!__instance.Invincible && Data.Death.ReceiveDeath) {
                __instance.Die();
            }
        }
    }
}

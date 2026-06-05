using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller.Death {
    [HarmonyPatch(typeof(PlayerController), "State", MethodType.Getter)]
    public static class PlayerState {
        public static void Postfix(PlayerController __instance) {
            if (Data.DeathLink.ReceiveDeath) {
                __instance.Die();
            }
        }
    }
}

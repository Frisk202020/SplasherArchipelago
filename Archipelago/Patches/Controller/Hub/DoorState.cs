using HarmonyLib;

/**
 * Redirect behavior of Time Attack doors if needed.
 */
namespace SplasherArchipelago.Patches.Controller.Hub {
    [HarmonyPatch(typeof(Door), "State", MethodType.Getter)]
    public static class GetDoorState {
        public static bool Prefix(Door __instance, ref HubDoorState __result) {
            __result = Data.SaveData.GetDoorState(__instance, GameManager.Mode == GameMode.TimeAttack);
            return false;
        }
    }

    [HarmonyPatch(typeof(Door), "State", MethodType.Setter)]
    public static class SetDoorState {
        public static bool Prefix(Door __instance, ref HubDoorState value) {
            value = Data.SaveData.GetDoorState(__instance, GameManager.Mode == GameMode.TimeAttack);
            return true;
        }
    }
}
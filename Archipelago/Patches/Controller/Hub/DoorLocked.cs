using HarmonyLib;

/**
 * Bypass the golden splasher requirement to unlock a Time Attack level.
 */

namespace SplasherArchipelago.Patches.Controller.Hub {
    [HarmonyPatch(typeof(Door), "Locked", MethodType.Getter)]
    public static class DoorLocked {
        public static bool Prefix(Door __instance, ref bool __result) {
            __result = __instance.State == HubDoorState.Locked;
            return false;
        }
    }
}

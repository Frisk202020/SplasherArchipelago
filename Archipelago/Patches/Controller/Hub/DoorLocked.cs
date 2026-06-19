using HarmonyLib;

/**
 * Bypass the golden splasher requirement to unlock a Time Attack level.
 */

namespace SplasherArchipelago.Patches.Controller.Hub {
    [HarmonyPatch(typeof(global::Door), "Locked", MethodType.Getter)]
    internal static class DoorLocked {
        public static bool Prefix(global::Door __instance, ref bool __result) {
            __result = __instance.State == HubDoorState.Locked;
            return false;
        }
    }
}

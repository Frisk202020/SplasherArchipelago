using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller.Hub {
    [HarmonyPatch(typeof(Door), "Locked", MethodType.Getter)]
    internal static class DoorLocked {
        public static bool Prefix(Door __instance, ref bool __result) {
            __result = __instance.State == HubDoorState.Locked;
            return false;
        }
    }
}

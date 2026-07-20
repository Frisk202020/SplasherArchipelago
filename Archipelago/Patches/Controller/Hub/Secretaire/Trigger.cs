using HarmonyLib;

namespace Archipelago.Patches.Controller.Hub.Secretaire {
    [HarmonyPatch(typeof(HubSecretaire), "OnGenericTriggerEnter")]
    public static class Trigger {
        public static bool Prefix(HubSecretaire __instance) {
            if (Actor.Instance is null) return true;

            Actor.Instance.Call(__instance);
            return false;
        }
    }
}

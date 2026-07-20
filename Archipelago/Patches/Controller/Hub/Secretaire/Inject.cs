using HarmonyLib;

namespace Archipelago.Patches.Controller.Hub.Secretaire {
    [HarmonyPatch(typeof(HubSecretaire), "Start")]
    public static class Inject {
        public static bool Prefix(HubSecretaire __instance) {
            __instance.gameObject.AddComponent<Actor>();
            return true;
        }
    }
}

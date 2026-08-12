using HarmonyLib;

namespace Archipelago.Patches.Controller.Hub {
    [HarmonyPatch(typeof(HubHUD), "Update")]
    public static class HUDUpdate {
        public static bool Prefix(HubHUD __instance) {
            if (!Data.Items.Splashers.Update) return true;

            __instance.splasherText.text = Data.Items.Splashers.Text();
            return true;
        }
    }
}
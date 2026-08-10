using HarmonyLib;

namespace Archipelago.Patches.Controller.Hub {
    [HarmonyPatch(typeof(HubHUD), "Awake")]
    public static class HUDAwake {
        public static bool Prefix(HubHUD __instance) {
            HubHUD.Instance = __instance;
		    __instance.splasherText.text = Data.Items.Splashers.Count + "/" + Data.Items.Splashers.Goal;
            if (GameManager.Mode > GameMode.TimeAttack) __instance.mode.SetActive(value: false);

            return false;
        }
    }
}
using Archipelago.Helpers.Assets;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace Archipelago.Patches.UI {
    [Loader]
    [HarmonyPatch(typeof(HUD), "Start")]
    public static class TrackerHUD {
        [Asset]
        private static GameObject Tracker = null;

        [Asset]
        private static GameObject Tracker_BG = null;

        public static void Postfix(HUD __instance) {
            var obj = Object.Instantiate(Tracker);
            var bg = Object.Instantiate(Tracker_BG);
            var script = obj.AddComponent<Public.Tracker>();

            obj.transform.SetParent(__instance.gameObject.transform, false);
            bg.transform.SetParent(obj.transform, false);

            bg.gameObject.SetActive(false);
            script.Init(bg, bg.transform.GetChild(0).GetComponent<Text>());
        }
    }
}
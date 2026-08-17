using Archipelago.Helpers.Assets;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.UI {
    [HarmonyPatch(typeof(StarFillTrigger), "Start")]
    [Loader]
    public static class Jauge {
        [Asset("Jauge_NoCount.png")]
        private static Sprite[] jauge = null;
        
        public static void Postfix(StarFillTrigger __instance) {
            __instance.gameObject.GetComponent<SpriteRenderer>().sprite = jauge[0];
        }
    }
}

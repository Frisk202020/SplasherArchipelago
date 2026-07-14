using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.UI {
    [HarmonyPatch(typeof(StarFillTrigger), "Start")]
    public static class Jauge {
        public static void Postfix(StarFillTrigger __instance) {
            __instance.gameObject.GetComponent<SpriteRenderer>().sprite = Data.UI.Sprites.Jauge;
        }
    }
}

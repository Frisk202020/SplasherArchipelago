using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.UI {
    [HarmonyPatch(typeof(AnimatedHUDElement), "ShowAndHide")]
    public static class ScoreOver {
        public static bool Prefix(AnimatedHUDElement __instance) {
            if (__instance.gameObject.name != "Coins") return true;

            HUD.Instance.dropCoins.gameObject.transform.Find("ScoreOver").GetComponent<TextMesh>().text = $"/ {GameData.Instance.CollectableData.StarFillCount}";
            return true;
        }
    }
}

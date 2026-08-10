using HarmonyLib;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Archipelago.Patches.UI.Camera {
    [HarmonyPatch(typeof(PlayerCamera), "Start")]
    public static class Animator {
        public static void Postfix(PlayerCamera __instance) {
            Data.UI.Camera.SetAnim(__instance);
            
            var colorScript = __instance.gameObject.GetComponent<ColorCorrectionCurves>();
            colorScript.redChannel = AnimationCurve.Linear(0, 0, 1, .5f);
            colorScript.blueChannel = AnimationCurve.Linear(0, 0, 1, .5f);
            colorScript.greenChannel = AnimationCurve.Linear(0, 0, 1, 1);
            colorScript.mode = ColorCorrectionCurves.ColorCorrectionMode.Simple;

            colorScript.UpdateParameters();
        }
    }
}
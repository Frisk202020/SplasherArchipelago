using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Archipelago.Data.UI {
    internal static class Camera {
        private class VanillaCurves {
            internal AnimationCurve red;
            internal AnimationCurve green;
            internal AnimationCurve blue;
            internal ColorCorrectionCurves.ColorCorrectionMode mode;
        }
        private static VanillaCurves vanilla = null;


        private static RuntimeAnimatorController animator;
        private static readonly FieldInfo AccessAnimator = AccessTools.Field(typeof(PlayerCamera), "effectAnimator");

        internal static void Load(AssetBundle bundle) {
            animator = bundle.LoadAsset<RuntimeAnimatorController>("CameraEffect_AnimController");
            if (PlayerCamera.Instance != null) SetAnim(PlayerCamera.Instance);
        }

        internal static void SetAnim(PlayerCamera instance) {
            var field = (UnityEngine.Animator)AccessAnimator.GetValue(instance);
            field.runtimeAnimatorController = animator;
            field.Rebind();
        }

        internal static void UpdateCurves(PlayerCamera instance, float ratio, bool init) {
            Core.Static.Log("Update");
            var script = instance.gameObject.GetComponent<ColorCorrectionCurves>();
            var curve = AnimationCurve.Linear(0, 0, 1, ratio);

            if (vanilla == null) {
                vanilla = new VanillaCurves {
                    red = script.redChannel,
                    blue = script.blueChannel,
                    green = script.greenChannel,
                    mode = script.mode
                };
            }

            script.redChannel = curve;
            script.blueChannel = curve;

            if (init) {
                script.greenChannel = AnimationCurve.Linear(0, 0, 1, 1);
                script.mode = ColorCorrectionCurves.ColorCorrectionMode.Simple;
            }

            script.UpdateParameters();
        }

        internal static void ResetCurves(PlayerCamera __instance) {
            if (vanilla == null) return;

            var script = __instance.gameObject.GetComponent<ColorCorrectionCurves>();
            script.redChannel = vanilla.red;
            script.greenChannel = vanilla.green;
            script.blueChannel = vanilla.blue;
            script.mode = vanilla.mode;

            script.UpdateParameters();
        }
    }
}
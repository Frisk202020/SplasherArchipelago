using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Data.UI {
    internal static class Camera {
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
    }
}
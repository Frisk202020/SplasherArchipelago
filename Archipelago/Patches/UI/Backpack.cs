using HarmonyLib;
using System.Reflection;
using UnityEngine;

/**
 * Set the gun's appearance in accordance to unlocked powers.
 * Currently this can only be progressive so it will display the state of highest progression power.
 * This will require to add new sprites to behave properly.
 */

namespace Archipelago.Patches.UI {
    [HarmonyPatch(typeof(PlayerController), "Start")]
    public static class Backpack {
        private static readonly FieldInfo backpack = AccessTools.DeclaredField(typeof(PlayerController), "backpackAnimator");

        public static void Postfix(Animator ___backpackAnimator) {
            if (___backpackAnimator == null) return;

            ___backpackAnimator.runtimeAnimatorController = Data.UI.Animator.Backpack;
            UpdateAnimator(___backpackAnimator);
        }

        private static void UpdateAnimator(Animator ___backpackAnimator) {
            ___backpackAnimator.Rebind();
            ___backpackAnimator.Update(0f);

            ___backpackAnimator.SetInteger("WaterLevel", (int)Data.Items.Powers.WaterLevel);
            ___backpackAnimator.SetBool("Stickink", Data.Items.Powers.HasSticky);
            ___backpackAnimator.SetBool("Bouncink", Data.Items.Powers.HasBouncy);
        }

        public static void Update() {
            if (PlayerController.Instance == null) return;

            var animator = (Animator)backpack.GetValue(PlayerController.Instance);
            if (animator == null) return;

            UpdateAnimator(animator);
        }
    }
}
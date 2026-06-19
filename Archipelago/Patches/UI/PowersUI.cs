using HarmonyLib;
using UnityEngine;

/**
 * Set the gun's appearance in accordance to unlocked powers.
 * Currently this can only be progressive so it will display the state of highest progression power.
 * This will require to add new sprites to behave properly.
 */

namespace SplasherArchipelago.Patches.UI {
    [HarmonyPatch(typeof(PlayerController), "Start")]
    public static class Powers {
        public static void Postfix(PlayerController __instance, Animator ___backpackAnimator) {
            ___backpackAnimator.SetBool("Water", Data.Items.Powers.HasWater);
            ___backpackAnimator.SetBool("Stickink", Data.Items.Powers.HasSticky);
            ___backpackAnimator.SetBool("Bouncink", Data.Items.Powers.HasBouncy);
        }
    }
}
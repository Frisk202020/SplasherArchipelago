using HarmonyLib;
using UnityEngine;

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
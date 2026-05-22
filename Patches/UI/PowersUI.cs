using HarmonyLib;
using UnityEngine;

namespace SplasherArchipelago.Patches.UI {
    [HarmonyPatch(typeof(PlayerController), "Start")]
    public static class Powers {
        public static void Postfix(PlayerController __instance, Animator ___backpackAnimator) {
            ___backpackAnimator.SetBool("Water", Data.Powers.HasWater);
            ___backpackAnimator.SetBool("Stickink", Data.Powers.HasSticky);
            ___backpackAnimator.SetBool("Bouncink", Data.Powers.HasBouncy);
        }
    }
}
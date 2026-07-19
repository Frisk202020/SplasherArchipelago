using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.UI {
    [HarmonyPatch(typeof(Door), "UpdateMedal")]
    public static class PlatinumDoor {
        private static Sprite[] normalSprites;

        private static bool IsDoorPlatinum(Door __instance) {
            if (__instance.levelData.State != HubDoorState.Finished) return false;

            foreach (var rescued in __instance.levelData.RescuedSplashers) {
                if (!rescued) return false;
            }

            var medal = __instance.levelMetaData.GetMatchingMedal(__instance.levelData.GetPersonalBest(GameMode.TimeAttack));
            if (medal < Data.Locations.Speedrun.HighestRequiredMedal) return false;

            return true;
        }

        public static bool Prefix(Door __instance) {
            if (normalSprites != null) return true;

            normalSprites = __instance.toReskin.Select(x => x.sprite).ToArray();
            return true;
        }

        public static void Postfix(Door __instance) {
            __instance.newTxt.SetActive(__instance.State == HubDoorState.Unlocked && GameManager.Mode == GameMode.Standard);
            if (__instance.newTxt.activeInHierarchy) {
                HubHUD.Instance.newDoorPosition = __instance.newTxt.transform.position;
            }

            var isPlatinum = IsDoorPlatinum(__instance);
            var sprites = isPlatinum
                    ? GameActor.GD.HubData.DoorReskin_Golden
                    : normalSprites;

            for (var i = 0; i < __instance.toReskin.Length; i++) {
                __instance.toReskin[i].sprite = sprites[i];
            }

            if (isPlatinum) {
                __instance.completeParticles.Play();
            } else {
                __instance.completeParticles.Stop();
            }
        }
    }
}

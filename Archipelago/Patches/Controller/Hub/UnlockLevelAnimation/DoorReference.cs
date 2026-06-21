using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

/**
 * Keep a reference to each Door that should be unlocked so we can queue unlock animations.
 * Vanilla enforces very much that only one door should unlock, so we need some setup to make it happen.
 */

namespace SplasherArchipelago.Patches.Controller.Hub.UnlockLevelAnimation {
    [HarmonyPatch(typeof(Door), "Start")]
    public static class DoorReference {
        private static readonly Dictionary<string, Door> doors = new Dictionary<string, Door>();

        public static bool Prefix(Door __instance) {
            doors[__instance.levelMetaData.SceneName] = __instance;
            return true;
        }

        public static void Postfix(Door __instance, TextMesh ___txt2) {
            if (!Data.Items.LevelKeys.ShowName || __instance.State == HubDoorState.Finished) return;
            ___txt2.text = $"{GameActor.GD.GetLevelNumber(__instance.levelMetaData)} - {__instance.levelMetaData.LevelName.GetString()}";
        }

        public static void StartUnlock(int id) {
            var key = GameData.Instance.LevelMetaDataList[id].SceneName;
            if (!doors.ContainsKey(key)) return;

            doors[GameData.Instance.LevelMetaDataList[id].SceneName].StartCoroutine("CoroutineUnlockFlip");
        }
    }
}

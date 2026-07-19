using HarmonyLib;
using Archipelago.Data;
using System.Collections.Generic;
using UnityEngine;

/**
 * Keep a reference to each Door that should be unlocked so we can queue unlock animations.
 * Vanilla enforces very much that only one door should unlock, so we need some setup to make it happen.
 */

namespace Archipelago.Patches.Controller.Hub.UnlockLevelAnimation {
    [HarmonyPatch(typeof(Door), "Start")]
    public static class DoorReference {
        private static readonly Dictionary<string, Door> doors = new Dictionary<string, Door>();
        internal static PendingKeyUnlock UnlockOccurring = null;

        public static bool Prefix(Door __instance) {
            // cancel vanilla unlocks (will re-unlocked if key actually in queue
            if (
                global::Hub.UnlockingLevel == __instance.levelMetaData.SceneName &&
                GameData.Instance.GetLevelData(__instance.levelMetaData.SceneName).State == HubDoorState.Unlocked
            ) {
                SaveData.SetDoorState(__instance, HubDoorState.Locked, false, true);
                global::Hub.UnlockingLevel = string.Empty;
            }
                 

            doors[__instance.levelMetaData.SceneName] = __instance;
            return true;
        }

        public static void Postfix(Door __instance, TextMesh ___txt2) {
            if (Data.Items.LevelKeys.ShowName && __instance.State != HubDoorState.Finished)
                ___txt2.text = $"{GameActor.GD.GetLevelNumber(__instance.levelMetaData)} - {__instance.levelMetaData.LevelName.GetString()}";
        }

        private static void SetCorrectMode(bool isSpeedrun) {
            if (isSpeedrun && GameManager.Mode == GameMode.Standard) {
                GameManager.Mode = GameMode.TimeAttack;
            } else if (!isSpeedrun && GameManager.Mode == GameMode.TimeAttack) {
                GameManager.Mode = GameMode.Standard;
            } else return;

            AccessTools.Method(typeof(HubHUD), "PlayChangeModeFeedback").Invoke(HubHUD.Instance, new object[] {});
        }

        public static void TryUnlock() {
            if (UnlockOccurring != null) return;

            var unlock = Data.Items.LevelKeys.GetPendingUnlock();
            if (unlock is null || SaveData.GetDoorState(unlock.id, unlock.isSpeedrun) != HubDoorState.Locked) return;

            var key = GameData.Instance.LevelMetaDataList[unlock.id].SceneName;
            if (!doors.ContainsKey(key)) return;

            UnlockOccurring = unlock;
            SetCorrectMode(unlock.isSpeedrun);
            GameManager.LockControl = LockControlType.NoInputs;

            var door = doors[GameData.Instance.LevelMetaDataList[unlock.id].SceneName];
            SaveData.SetDoorState(door, HubDoorState.Unlocked, unlock.isSpeedrun, true);
            door.StartCoroutine("CoroutineUnlockFlip");
        }
    }
}

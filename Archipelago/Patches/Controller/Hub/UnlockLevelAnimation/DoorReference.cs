using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/**
 * Keep a reference to each Door that should be unlocked so we can queue unlock animations.
 * Vanilla enforces very much that only one door should unlock, so we need some setup to make it happen.
 */

namespace SplasherArchipelago.Patches.Controller.Hub.UnlockLevelAnimation {
    [HarmonyPatch(typeof(Door), "Start")]
    public static class DoorReference {
        private static readonly Dictionary<string, Door> doors = new Dictionary<string, Door>();
        private static readonly MethodInfo StateSetter = AccessTools.DeclaredPropertySetter(typeof(Door), "State");

        private static int DoorsLoaded = 0;

        private static void SetDoorState(Door door, HubDoorState state) {
            StateSetter.Invoke(door, new object[] { state });
            GameData.Instance.GetLevelData(door.levelMetaData.SceneName).State = state;

            if (state == HubDoorState.Locked)
                GameData.Instance.SavePlayerData();
        }

        public static bool Prefix(Door __instance) {
            Data.HubState.DoorsLoaded = true;

            // cancel vanilla unlocks (will re-unlocked if key actually in queue
            if (
                global::Hub.UnlockingLevel == __instance.levelMetaData.SceneName &&
                GameData.Instance.GetLevelData(__instance.levelMetaData.SceneName).State == HubDoorState.Unlocked
            ) {
                SetDoorState(__instance, HubDoorState.Locked);
                global::Hub.UnlockingLevel = string.Empty;
            }
                 

            doors[__instance.levelMetaData.SceneName] = __instance;
            return true;
        }

        public static void Postfix(Door __instance, TextMesh ___txt2) {
             if (Data.Items.LevelKeys.ShowName && __instance.State != HubDoorState.Finished)
                ___txt2.text = $"{GameActor.GD.GetLevelNumber(__instance.levelMetaData)} - {__instance.levelMetaData.LevelName.GetString()}";

            // check if all doors are loaded for this hub loading sequence
            DoorsLoaded++;
            DoorsLoaded %= 22;
            if (DoorsLoaded != 0) return;


            var pending = Data.Items.LevelKeys.GetPendingUnlock();
            if (pending is null) return;

            StartUnlock(pending.Value);
        }

        public static void StartUnlock(int id) {
            var key = GameData.Instance.LevelMetaDataList[id].SceneName;
            if (!doors.ContainsKey(key)) return;

            var door = doors[GameData.Instance.LevelMetaDataList[id].SceneName];
            SetDoorState(door, HubDoorState.Unlocked);
            door.StartCoroutine("CoroutineUnlockFlip");
        }
    }
}

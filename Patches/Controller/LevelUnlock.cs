using HarmonyLib;
using System;

namespace SplasherArchipelago.Patches.Controller {
    [HarmonyPatch(typeof(PlayerSaveData), "GetLevelData")]
    public static class LevelUnlock {
        public static bool Prefix(PlayerSaveData __instance, int levelIndex) {
            if (__instance.LevelDataList[levelIndex].State is HubDoorState.Locked && Data.LevelKeys.IsLevelUnlocked(levelIndex)) {
                __instance.LevelDataList[levelIndex].State = HubDoorState.Unlocked;
            }

            return true;    
        }
    }
}
using HarmonyLib;

/**
 * Setup the unlock animation to unlock one level of the queue, or none if no level is in queue.
 * When finished, more animations start if the queue is not empty (@see UnlockMoreLevels)
 */

namespace SplasherArchipelago.Patches.Controller.Hub.UnlockLevelAnimation {
    [HarmonyPatch(typeof(global::Hub), "CoroutineRefreshUnlocks")]
    public static class UnlockingLevel {
        public static bool Prefix() {
            var pending = Data.Items.LevelKeys.GetPendingUnlock();
            global::Hub.UnlockingLevel = pending is null ? string.Empty : GameData.Instance.LevelMetaDataList[pending.Value + 1].SceneName;
            return true;
        }
    }
}

using HarmonyLib;
using System.Collections.Generic;

/**
 * Keep a reference to each Door that should be unlocked so we can queue unlock animations.
 * Vanilla enforces very much that only one door should unlock, so we need some setup to make it happen.
 */

namespace SplasherArchipelago.Patches.Controller.Hub.UnlockLevelAnimation {
    [HarmonyPatch(typeof(global::Door), "Start")]
    public static class RemainingUnlocks {
        private static HashSet<string> pendingUnlocks = null;
        private static Queue<global::Door> pendingDoors = new Queue<global::Door>();

        public static bool Prefix(global::Door __instance, LevelMetaData ___levelMetaData) {
            if (pendingUnlocks is null) {
                var ids = Data.Items.LevelKeys.PendingUnlockSet();
                pendingUnlocks = new HashSet<string>();

                foreach (var id in ids) {
                    pendingUnlocks.Add(GameData.Instance.LevelMetaDataList[id + 1].SceneName);
                }
            }

            if (pendingUnlocks.Count > 0 && pendingUnlocks.Contains(___levelMetaData.SceneName)) {
                pendingDoors.Enqueue(__instance);
            }

            return true;
        }

        internal static global::Door GetNextUnlock() {
            if (pendingUnlocks != null) pendingUnlocks = null;
            if (pendingDoors.Count == 0) return null;

            return pendingDoors.Dequeue();
        }
    }
}

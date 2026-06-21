using HarmonyLib;
using System.Reflection;

/**
 * Launch more unlock animations in queue when the current one finishes.
 * This patch just captures the end of the coroutine.
 */

namespace SplasherArchipelago.Patches.Controller.Hub.UnlockLevelAnimation {
    [HarmonyPatch]
    public static class UnlockMoreLevels {
        [HarmonyTargetMethod]
        public static MethodBase Target() {
            return AccessTools.Method(typeof(Door).GetNestedType("<CoroutineUnlockFlip>c__Iterator1", BindingFlags.NonPublic), "MoveNext");
        }

        [HarmonyPostfix]
        public static void Postfix(bool __result) {
            if (__result) return;

            var pending = Data.Items.LevelKeys.GetPendingUnlock();
            if (pending is null) {
                GameData.Instance.SavePlayerData();
                return;
            }

            DoorReference.StartUnlock(pending.Value);
        }
    }
}

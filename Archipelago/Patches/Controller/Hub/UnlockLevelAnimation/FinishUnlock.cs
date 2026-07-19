using HarmonyLib;
using System.Reflection;

/**
 * Launch more unlock animations in queue when the current one finishes.
 * This patch just captures the end of the coroutine.
 */

namespace Archipelago.Patches.Controller.Hub.UnlockLevelAnimation {
    [HarmonyPatch]
    public static class FinishUnlock {
        [HarmonyTargetMethod]
        public static MethodBase Target() {
            return AccessTools.Method(typeof(Door).GetNestedType("<CoroutineUnlockFlip>c__Iterator1", BindingFlags.NonPublic), "MoveNext");
        }

        [HarmonyPostfix]
        public static void Postfix(bool __result) {
            if (__result) return;

            GameData.Instance.SavePlayerData();
            DoorReference.UnlockOccurring = null;
            GameManager.LockControl = LockControlType.None;
            Core.Data.Time.Clean();
        }
    }
}

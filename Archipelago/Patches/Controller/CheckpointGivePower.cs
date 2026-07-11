using HarmonyLib;
using System.Reflection;

/**
 * Make the game believe powers are not unlocked when spawning a power unlock in a level because overwhise it will not spawn.
 */

namespace Archipelago.Patches.Controller {
    [HarmonyPatch]
    public static class CheckpointGivePower {
        private static void SetAll(bool x) {
            PlayerController.Instance.Machine.water = x;
            PlayerController.Instance.Machine.stickyPaint = x;
            PlayerController.Instance.Machine.bouncyPaint = x;
        }
        
        [HarmonyTargetMethod]
        public static MethodBase TargetMethod() {
            return AccessTools.Method(typeof(CheckpointNewPower).GetNestedType("<CoroutineEnablePower>c__Iterator0", BindingFlags.NonPublic), "MoveNext");
        }

        [HarmonyPrefix]
        public static bool Prefix() {
            SetAll(false);
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix() {
            SetAll(true);
            return;
        }
    }
}

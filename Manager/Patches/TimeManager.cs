using HarmonyLib;
using UnityEngine;

namespace SplasherManager.Patches {
    [HarmonyPatch(typeof(GameManager), "LockControl", MethodType.Setter)]
    public static class TimeManager {
        public static bool Prefix(LockControlType value) {
            if (
                (Exit.Instance is null || !Exit.Instance.LevelEnded) &&
                (value == LockControlType.NoInputs || value == LockControlType.FreezeAll)
            ) {
                Time.timeScale = Data.Time.TimeScale;
                return true;
            }


            if (Time.timeScale > 1 && GameManager.LockControl == LockControlType.FreezeAll) {
                Data.Time.Clean();
            }

            return true;
        }
    }
}

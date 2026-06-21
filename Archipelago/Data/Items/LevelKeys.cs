using System.Collections.Generic;

namespace SplasherArchipelago.Data.Items {
    class LevelKeys {
        private const int Levels = 21; // lvl 1 is always unlocked
        private readonly static Queue<int> pendingUnlocks = new Queue<int>();

        internal static bool ShowName = false;

        private static bool SetState(LevelData data) {
            if (data.State == HubDoorState.Locked)
                data.State = HubDoorState.Unlocked;

            return false;
        }

        internal static void UnlockFirst() {
            SetState(GameData.Instance.CurrentPlayerData.LevelDataList[0]);
        }

        internal static void Unlock(int id) {
            if (id >= Levels) return;

            var inGameId = id + 1;
            var data = GameData.Instance.CurrentPlayerData.LevelDataList[inGameId];
            if (!SetState(data)) return;

            if (
                Hub.IsLoaded &&
                Hub.Instance != null &&
                Hub.Instance.doors != null &&
                GameManager.LockControl == LockControlType.None &&
                !PlayerCamera.Instance.IgnoreZoneContraints
            ) {
                Patches.Controller.Hub.UnlockLevelAnimation.DoorReference.StartUnlock(inGameId);
            } else {
                pendingUnlocks.Enqueue(inGameId);
            }
        }

        internal static int? GetPendingUnlock() {
            if (pendingUnlocks.Count == 0) return null;
            
            var x = pendingUnlocks.Dequeue();
            return x;
        }

        internal static void UnlockAll() {
            var levels = GameData.Instance.CurrentPlayerData.LevelDataList;
            foreach (var level in levels) {
                SetState(level);
            }
        }
    }
}
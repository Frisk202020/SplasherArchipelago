using System.Collections.Generic;

namespace SplasherArchipelago.Data.Items {
    class LevelKeys {
        private const int Levels = 21; // lvl 1 is always unlocked
        private readonly static Queue<int> pendingUnlocks = new Queue<int>();

        internal static bool ShowName = false;

        internal static void UnlockFirst() {
            var data = GameData.Instance.CurrentPlayerData.LevelDataList[0];

            if (data.State == HubDoorState.Locked)
                data.State = HubDoorState.Unlocked;
        }

        internal static void Unlock(int id) {
            if (id >= Levels) return;

            var inGameId = id + 1;
            var data = GameData.Instance.CurrentPlayerData.LevelDataList[inGameId];
            if (data.State != HubDoorState.Locked) return;

            if (
                Hub.IsLoaded &&
                HubState.DoorsLoaded &&
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
                if (level.State == HubDoorState.Locked)
                    level.State = HubDoorState.Unlocked;
            }
        }
    }
}
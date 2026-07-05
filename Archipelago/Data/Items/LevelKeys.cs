using System.Collections.Generic;

namespace SplasherArchipelago.Data.Items {
    class LevelKeys {
        private const int Levels = 21; // lvl 1 is always unlocked
        private readonly static Queue<PendingKeyUnlock> pendingUnlocks = new Queue<PendingKeyUnlock>();

        internal static bool ShowName = false;

        internal static void UnlockFirst() {
            var data = GameData.Instance.CurrentPlayerData.LevelDataList[0];

            if (data.State == HubDoorState.Locked)
                data.State = HubDoorState.Unlocked;
        }

        internal static void Unlock(int id, bool speedrun) {
            if (id >= Levels) return;

            var inGameId = id + 1;
            if (SaveData.GetDoorState(inGameId, speedrun) != HubDoorState.Locked) return;

            pendingUnlocks.Enqueue(new PendingKeyUnlock(inGameId, speedrun));
        }

        internal static PendingKeyUnlock GetPendingUnlock() {
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
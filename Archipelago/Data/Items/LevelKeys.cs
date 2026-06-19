using System.Collections.Generic;

namespace SplasherArchipelago.Data.Items {
    class LevelKeys {
        private const int Levels = 21; // lvl 1 is always unlocked
        private static bool[] Keys { get; } = new bool[Levels];

        private static Queue<int> pendingUnlocks = new Queue<int>();

        internal static void Unlock(int id) {
            if (id < Levels && !Keys[id]) {
                Keys[id] = true;
                pendingUnlocks.Enqueue(id);
            }
        }

        internal static int? GetPendingUnlock() {
            if (pendingUnlocks.Count == 0) return null;
            
            var x = pendingUnlocks.Dequeue();
            return x;
        }

        internal static HashSet<int> PendingUnlockSet() {
            var set = new HashSet<int>();
            while (pendingUnlocks.Count > 0) {
                set.Add(pendingUnlocks.Dequeue());
            }

            return set;
        }

        internal static void UnlockAll() {
            for (int i = 0; i < Levels; i++) {
                Keys[i] = true;
            }
        }

        internal static bool IsLevelUnlocked(int id) {
            return id >= 0 && id < Levels && Keys[id];
        }
    }
}
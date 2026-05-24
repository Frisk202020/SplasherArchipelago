using System;

namespace SplasherArchipelago.Data {
    public class LevelKeys {
        private static int Levels = 22;
        private static bool[] keys { get; } = new bool[Levels];

        public static void Unlock(uint id) {
            if (id < Levels) {
                keys[id] = true;
            }
        }

        public static void UnlockAll() {
            for (int i = 1; i < Levels; i++) {
                keys[i] = true;
            }
        }

        public static bool IsLevelUnlocked(int id) {
            return id >= 0 && id < Levels && keys[id];
        }
    }
}
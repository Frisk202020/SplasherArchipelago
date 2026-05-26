namespace SplasherArchipelago.Data.Items {
    class LevelKeys {
        private static int Levels = 22;
        private static bool[] Keys { get; } = new bool[Levels];

        internal static void Unlock(uint id) {
            if (id < Levels) {
                Keys[id] = true;
            }
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
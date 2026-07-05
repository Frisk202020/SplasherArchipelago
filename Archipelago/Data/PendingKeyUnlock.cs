namespace SplasherArchipelago.Data {
    internal class PendingKeyUnlock {
        public readonly int id;
        public readonly bool isSpeedrun;

        public PendingKeyUnlock(int id, bool isSpeedrun) {
            this.id = id;
            this.isSpeedrun = isSpeedrun;
        }
    }
}

namespace Archipelago.Data {
    internal static class DeathLink {
        internal static uint Trigger = 4;
        private static bool hero = false;

        internal static void SetHero() => hero = true;

        private static void Send() {
            Count = 0;
            Network.ArchipelagoManager.SendDeathLink();
        }

        public static bool ReceiveDeath { get; private set; } = false;
        private static uint Count = 0;
        internal static void AddDeath() {
            if (ReceiveDeath) {
                ReceiveDeath = false;
                return;
            }

            if (Count == Trigger) {
                Send();
                return;
            }

            Count++;
        }

        internal static void ReportSplasherDeath() {
            if (!hero) return;
            
            AddDeath();
            ReceiveDeath = true;
        }

        internal static void ReceiveDeathLink(Archipelago.MultiClient.Net.BounceFeatures.DeathLink.DeathLink death) {
            Core.Static.Log($"Died from {death.Source} ({death.Cause})");

            Count = 0;
            ReceiveDeath = true;
        }
    }
}

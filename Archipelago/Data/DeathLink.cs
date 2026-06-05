namespace SplasherArchipelago.Data {
    internal static class DeathLink {
        internal static uint Trigger = 4;
        internal static bool TriggerOnSplasherDeath { get; private set; } = false;
        internal static void EnableGodMode() { TriggerOnSplasherDeath = true; }

        private static void Send() {
            Count = 0;
            Network.ArchipelagoManager.SendDeathLink();
        }

        public static bool ReceiveDeath { get; private set; } = false;
        private static uint Count = 0;
        internal static void AddDeath() {
            Util.Log($"{Count} {Trigger}");
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
            if (TriggerOnSplasherDeath) {
                Send();
            }
        }

        internal static void ReceiveDeathLink(Archipelago.MultiClient.Net.BounceFeatures.DeathLink.DeathLink death) {
            Util.Log($"Died from {death.Source} ({death.Cause})");
            Count = 0;
            ReceiveDeath = true;
        }
    }
}

namespace SplasherArchipelago.Data {
    internal static class DeathLink {
        internal static uint Trigger = 4;

        private enum Legend {
            None = 0,
            Selfish = 1,
            Absolute = 2
        }
        internal static void SetSelfish() { legend = Legend.Selfish;  }
        internal static void SetAbsoluteLegend() { legend = Legend.Absolute; }
        private static Legend legend = Legend.None;

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
            switch(legend) {
                case Legend.Absolute: ReceiveDeath = true; goto case Legend.Selfish;
                case Legend.Selfish: Send(); break;
            }
        }

        internal static void ReceiveDeathLink(Archipelago.MultiClient.Net.BounceFeatures.DeathLink.DeathLink death) {
            Util.Log($"Died from {death.Source} ({death.Cause})");
            Count = 0;
            ReceiveDeath = true;
        }
    }
}

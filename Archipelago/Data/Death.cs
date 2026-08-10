namespace Archipelago.Data {
    internal static class Death {
        internal static uint DeathLinkAmnesty = 5;
        internal static uint TrapAmnesty = 5;
        private static bool hero = false;

        internal static void SetHero() => hero = true;

        public static bool ReceiveDeath { get; private set; } = false;
        private static uint Count = 0;
        private static uint? TrapCount = null;

        internal static void StartTrapCount() => TrapCount = 0;

        internal static void AddDeath() {
            if (ReceiveDeath) {
                ReceiveDeath = false;
                return;
            }

            Count++;
            if (TrapCount != null) TrapCount++;

            if (Count % DeathLinkAmnesty == 0) {
                Network.ArchipelagoManager.SendDeathLink();
            } 

            if (TrapCount != null && TrapCount % TrapAmnesty == 0) {
                TrapController.Free();
                TrapCount = null;
            }
        }

        internal static void ReportSplasherDeath() {
            if (!hero) return;
            
            AddDeath();
            ReceiveDeath = true;
        }

        internal static void ReceiveDeathLink(MultiClient.Net.BounceFeatures.DeathLink.DeathLink death) {
            Core.Static.Log($"Died from {death.Source} ({death.Cause})");

            Count = 0;
            ReceiveDeath = true;
        }
    }
}

namespace Archipelago.Data.Items {
    static class Freedom {
        internal static bool IsFree { get; private set; } = false;
        internal static void Free() {
            if (IsFree) return;

            Network.ArchipelagoManager.Victory();
            IsFree = true; 
        }
    }
}

namespace Archipelago.Network.Helpers {
    internal class HintInfo {
        internal readonly string location;
        internal readonly string player;
        internal readonly string item;
        internal readonly bool local;

        internal HintInfo(string location, string player, string item, bool local) {
            this.location = location;
            this.player = player;
            this.item = item;
            this.local = local;
        }
    }
}

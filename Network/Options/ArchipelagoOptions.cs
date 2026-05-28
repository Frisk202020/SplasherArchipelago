namespace SplasherArchipelago.Network.Options {
    internal class ArchipelagoOptions {
        public int Splashers_Goal { get; set; }

        public void Apply() {
            Data.Items.Splashers.Goal = Splashers_Goal;
        }
    }
}

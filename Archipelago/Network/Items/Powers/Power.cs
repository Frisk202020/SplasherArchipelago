namespace SplasherArchipelago.Network.Items.Powers {
    abstract class Power {
        public virtual string Name() => "Unlock";

        public bool CollectOnStart() { return true; }
    }
}
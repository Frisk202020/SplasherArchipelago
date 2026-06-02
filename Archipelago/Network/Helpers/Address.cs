namespace SplasherArchipelago.Network.Helpers {
    internal class Address {
        public string domain;
        public int port;

        public override string ToString() {
            return $"{domain}:{port}";
        }
    }
}

namespace SplasherArchipelago.Data {
    static class Essence {
        private const uint max = 700;
        private static uint ammount = 0;

        public static void Add(uint n) => ammount += n;
        public static uint Release() {
            if (ammount <= max) {
                var ret = ammount;
                ammount = 0;
                return ret;
            }

            ammount -= max;
            return max;
        }
    }
}

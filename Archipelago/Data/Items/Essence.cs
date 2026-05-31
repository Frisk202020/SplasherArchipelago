namespace SplasherArchipelago.Data.Items {
    static class Essence {
        public const uint MAX = 700;
        private static uint ammount = 0;

        public static void Add(uint n) => ammount += n;
        public static uint Release() {
            if (ammount <= MAX) {
                var ret = ammount;
                ammount = 0;

                return ret;
            }

            ammount -= MAX;
            return MAX;
        }
    }
}

namespace SplasherArchipelago.Data {
    public static class Splashers {
        public static int Count { get; private set; } = 0;
        public static void Add() { Count++; }
    }
}
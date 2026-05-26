namespace SplasherArchipelago.Data.Items {
    static class Splashers {
        internal static int Count { get; private set; } = 0;
        internal static void Add() { Count++; }
    }
}
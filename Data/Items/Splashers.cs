namespace SplasherArchipelago.Data.Items {
    static class Splashers {
        internal static int BadEncounter { get; set; } = 0; // to implement in options
        internal static int Sncf { get; set; } = 0; // to implement in options
        internal static int Goal { get; set; } = 0;

        internal static int Count { get; private set; } = 0;
        internal static void Add() { Count++; }
    }
}
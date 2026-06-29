using HarmonyLib;
using SplasherArchipelago.Network;
using SplasherArchipelago.Public;
using System;
using System.Collections.Generic;

namespace SplasherArchipelago {
    public static class Shared {
        public const string pluginId = "com.frisk.splahser_archipelago";
        public readonly static Version version = new Version(0, 0, 6);

        public const string VANILLA_FILE = "Save1";

        internal static string Seed = "";
        public static string SaveFile() => $"Archipelago_{Seed}";
        public static string SaveFileExtension() => SaveFile() + "_Extension";


        public delegate void VoidHandler();
        public static event VoidHandler CreditsEvent;

        public delegate void ConfigHandler(Config config);
        public static event ConfigHandler OnConfigParsed;

        internal static void StartCreditsEvents() {
            if (CreditsEvent is null) return;
            CreditsEvent();
        }

        internal static void StartConfigEvents(Config config) {
            if (OnConfigParsed is null) return;
            OnConfigParsed(config);
        }

        private static readonly Harmony harmony = new Harmony(pluginId);
        public static bool Start() {
            if (ArchipelagoManager.Start()) {
                harmony.PatchAll();
                return true;
            }
            return false;
        }

        public static Config Config { get; internal set; } = null;
        public static Config Parse() {
            var config = Config.Parse();
            if (config is null) return null;

            Config = config;
            return config;
        }
    }
}

using HarmonyLib;
using SplasherArchipelago.Network;
using SplasherArchipelago.Public;

namespace SplasherArchipelago {
    public static class Shared {
        public const string pluginId = "com.frisk.splahser_archipelago";

        private static Harmony harmony = new Harmony(pluginId);
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

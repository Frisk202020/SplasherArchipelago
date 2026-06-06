using HarmonyLib;
using HarmonyLib.Tools;
using SplasherArchipelago.Network;

namespace SplasherArchipelago {
    public static class Util {
        public const string pluginId = "com.frisk.splahser_archipelago";
        internal const string Game = "Splasher";
        internal const long BaseId = 0xF4A201;
        internal const uint LevelCount = 22;

        private static BepInEx.Logging.ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource("Archipelago");

        internal readonly static string[] Levels = {
            "Welcome to Inkorp", "Potatoes Ink", "Stick To The Plan",
            "Let It Bounce", "Jump On The Water", "A Bad Encounter",
            "There Will Be Fries", "Ray Man Origin", "Stick On The Water",
            "Ink In  Park", "Wind Walker", "Troopers Please",
            "Water Is Coming", "Inkorp Express", "Big Bounce Theory",
            "Toxink Bubbles", "Storm Wind", "Ray Man Legend",
            "Toxink Avenger", "The Glados Principle", "Apocalink Now",
            "Good Luck Splasher"
        };

        internal static Harmony harmony = new Harmony(pluginId);

        public static bool Start() {
            if (ArchipelagoManager.Start()) {
                harmony.PatchAll();
                return true;
            }
            return false;
        }

        public static void Log(string msg) {
            logger.LogInfo(msg);
        }

        public static void Warn(string msg) {
            logger.LogWarning(msg);
        }

        public static void Error(string msg) {
            logger.LogError(msg);
        }
    }
}
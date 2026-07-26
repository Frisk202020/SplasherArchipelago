using HarmonyLib;
using TSKGames.Inputs;

namespace Archipelago {
    internal static class Util {
        internal const long BaseId = 0xF4A201;
        internal const uint LevelCount = 22;
        internal const string PluginId = Core.Static.PluginIdRoot + ".archipelago";
        internal const string FALLBACK_CATEGORY = "ArchipelagoFallbacks";

        internal static Harmony Harmony = new Harmony(PluginId);

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

        internal static string Seed = "";
        internal static string SaveFile() => $"Archipelago_{Seed}";
        internal static string SaveFileExtension() => SaveFile() + "_Extension";

        internal static readonly InputGamepadButton[] ShootButtons = new[] {
            GameManager.BUTTON_WATER, GameManager.BUTTON_STICKY, GameManager.BUTTON_BOUNCY
        };
    }
}
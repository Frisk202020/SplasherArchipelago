using HarmonyLib;
using TSKGames.PlatformSpecific.Steam;

namespace Archipelago.CorePatches {
    [HarmonyPatch(typeof(SteamCore), "IsSteamAvailable")]
    public static class ForbidSteamCloud {
        internal static bool Block = true;
        public static bool Prefix (ref bool __result) { 
            if (!Block) return true;

            __result = false;
            return false; 
        }
    }
}

using HarmonyLib;

namespace SplasherArchipelago.Patches {
    [HarmonyPatch(typeof(TitleScreen), "StartGame")]
    public static class Start {
        public static bool Prefix() {
            GameData.StartUILoading();
            Hub.Load();
            return false;
        }
    }
}
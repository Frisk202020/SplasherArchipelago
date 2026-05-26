using HarmonyLib;

namespace SplasherArchipelago.Patches.Setup {
    [HarmonyPatch(typeof(TitleScreen), "StartGame")]
    public static class Start {
        public static bool Prefix() {
            GameData.StartUILoading();
            Hub.Load();
            return false;
        }
    }
}
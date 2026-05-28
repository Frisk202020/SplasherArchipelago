using HarmonyLib;

namespace SplasherArchipelago.Patches.Setup {
    [HarmonyPatch(typeof(Hub), "Start")]
    public static class BossUnlock {
        private const int VANILLA_BAD_ENCOUNTER = 16;
        private const int VANILLA_SNCF = 50;
        private const int VANILLA_GOAL = 80;

        private static int? getCount(int vanilla_count) {
            switch(vanilla_count) {
                case VANILLA_BAD_ENCOUNTER:  return Data.Items.Splashers.BadEncounter;
                case VANILLA_SNCF: return Data.Items.Splashers.Sncf;
                case VANILLA_GOAL: return Data.Items.Splashers.Goal;
                default: return null;
            }
        }
    
        public static bool Prefix(Hub __instance) {
            foreach(var boss in __instance.bossUnlocks) {
                var count = getCount(boss.count);
                if (count is null) continue;

                boss.count = count.Value;
                boss.countText.text = count.ToString();
            }

            return true;
        }
    }
}

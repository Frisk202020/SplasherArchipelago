using HarmonyLib;

namespace Archipelago.Patches.UI {
    [HarmonyPatch(typeof(UIModeDescription), "SetupText")]
    public static class DoorLockedText {
        public static bool Prefix(UIModeDescription __instance, GameMode forMode) {
            if (forMode != GameMode.TimeAttack || Door.PlayerInFront is null) return true;
            __instance.modeText.text = GameData.Instance.HUDData.ModeStrings[(int)forMode].GetString();
            
            __instance.modeDesc.text = Helpers.Language.Get(
                "ArchipelagoLevelKeys", 
                "Desc", 
                Data.PendingKeyUnlock.KeyItemName(Door.PlayerInFront.levelMetaData.LevelName, true)
            );

            return false;
        }
    }
}

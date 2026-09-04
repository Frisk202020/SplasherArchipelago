using HarmonyLib;

namespace Archipelago.Patches.UI {
    [HarmonyPatch(typeof(HubHUD), "Update")]
    public static class HUDDoorUpdate {
        public static void Postfix() {
            if (
                Door.PlayerInFront is null || 
                Door.PlayerInFront.State != HubDoorState.Locked ||
                GameManager.Mode != GameMode.Standard ||
                UIModeDescription.Instance.IsActive
            ) return;

            UIModeDescription.Instance.modeText.text = GameData.Instance
                .HUDData.ModeStrings[(int)GameMode.Standard]
                .GetString();

            UIModeDescription.Instance.modeDesc.text = Helpers.Language.Get(
                "ArchipelagoLevelKeys", 
                "Desc", 
                Data.PendingKeyUnlock.KeyItemName(Door.PlayerInFront.levelMetaData.LevelName, false)
            );

            UIModeDescription.Instance.Show(true);
        }
    }
}

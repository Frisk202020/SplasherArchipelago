using HarmonyLib;

/**
 * Force Flip
 */

namespace Archipelago.Patches.Controller.Hub.UnlockLevelAnimation {
    [HarmonyPatch(typeof(HubHUD), "PlayChangeModeFeedback")]
    public static class ChangeModeFeedback {
        public static void Postfix() {
            foreach(var door in global::Hub.Instance.doors) {

                if (door.State == HubDoorState.Locked) {
                    door.medal.sprite = GameActor.GD.HubData.Locked_Sprite;
                    door.FlipShort();
                } 
            }
        }
    }
}
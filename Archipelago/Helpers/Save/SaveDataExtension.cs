using System;

namespace SplasherArchipelago.Helpers.Save {
    [Serializable]
    public class SaveDataExtension {
        public HubDoorState[] TimeAttackState;

        public SaveDataExtension() {
            TimeAttackState = new HubDoorState[Util.LevelCount];
            TimeAttackState[0] = HubDoorState.Unlocked;
        }
    }
}

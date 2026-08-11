using System;
using System.Collections.Generic;

namespace Archipelago.Helpers.Save {
    [Serializable]
    public class SaveDataExtension {
        public HubDoorState[] TimeAttackState;
        public List<long> CollectedIds = new List<long> {};

        public SaveDataExtension() {
            TimeAttackState = new HubDoorState[Util.LevelCount];
            TimeAttackState[0] = HubDoorState.Unlocked;
        }
    }
}

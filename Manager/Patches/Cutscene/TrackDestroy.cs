using UnityEngine;

namespace Manager.Patches.Cutscene {
    public class TrackDestroy : Core.Tools.VoidTracker {
        protected override bool IsResolved() => GameManager.LockControl == LockControlType.None;
    }
}

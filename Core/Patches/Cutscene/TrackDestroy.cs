namespace Core.Patches.Cutscene {
    public class TrackDestroy : Tools.VoidTracker {
        protected override bool IsResolved() => GameManager.LockControl == LockControlType.None;
    }
}

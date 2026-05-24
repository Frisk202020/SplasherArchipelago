namespace SplasherArchipelago.Patches.Listener {
    class Checkpoint : CheckpointListener {
        public void OnCheckpoint(global::Checkpoint c) {
            GameManager.Instance.StarManager.Add((int)Data.Essence.Release());
        }
    }
}

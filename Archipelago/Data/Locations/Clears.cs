namespace SplasherArchipelago.Data.Locations {
    internal static class Clears {
        private static bool Update(LevelData current) {
            if (current.State != HubDoorState.Finished) {
                current.State = HubDoorState.Finished;
                GameData.Instance.SavePlayerData();

                return true;
            }

            return false;
        }

        internal static void Check(LevelData current, int id) {
            if (Update(current))
                Network.ArchipelagoManager.Check(LocationType.Clear, id);
        }

        internal static void Restore(int id) {
            Update(GameData.Instance.CurrentPlayerData.LevelDataList[id - (int)LocationType.Clear]);
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace Archipelago.Data.Locations {
    class Checkpoint {
        private static readonly CheckpointTable table = new CheckpointTable();

        internal static void Check(string name) {
            if (table.Get(name)) return;
            
            table.Check(name);
            Network.ArchipelagoManager.Check(LocationType.Checkpoint, CheckpointTable.LocationId(name));
        }
    }
}
using System.Collections.Generic;

namespace Archipelago.Data.Items {
    internal static class Zone {
        internal class ZoneData {
            public string name;
            public HashSet<uint> keys;
        }

        internal static readonly ZoneData[] ZoneForLevel = new ZoneData[] {
            new ZoneData { name = "Reception Hub", keys = new HashSet<uint> { 0, 1, 2, 6 } },
            new ZoneData { name = "Water Pool", keys = new HashSet<uint> { 4, 5, 8, 12 } }, 
            new ZoneData { name = "Ray Man Paradise", keys = new HashSet<uint> { 7, 11, 17, 19 } },
            new ZoneData { name = "Toxink Hell", keys = new HashSet<uint> { 15, 18, 20 } },
            new ZoneData { name = "Inkorp Outskirts", keys = new HashSet<uint> { 10, 13, 16 } },
            new ZoneData { name = "Fun Park", keys = new HashSet<uint> { 3, 9, 14 } },
            new ZoneData { name = "Docteur's Office", keys = new HashSet<uint> { 21 } }
        };

        internal static string FindZone(uint id) {
            foreach(var zoneData in ZoneForLevel) {
                if (zoneData.keys.Contains(id)) return zoneData.name;
            }

            return "";
        }
    }
}
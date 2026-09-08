using System.Collections.Generic;
using Archipelago.Helpers;

namespace Archipelago.Data.Items {
    internal static class Zone {
        internal class ZoneData {
            internal string name;
            internal HashSet<uint> keys;

            internal ZoneData(string _name, params uint[] ids) {
                name = Language.Get("ArchipelagoZones", _name);
                keys = new HashSet<uint>(ids);
            }
        }

        internal static readonly ZoneData[] ZoneForLevel = new ZoneData[] {
            new ZoneData("reception", 0, 1, 2, 6),
            new ZoneData("pool", 4, 5, 8, 12),
            new ZoneData("paradise", 7, 11, 17, 19),
            new ZoneData("hell", 15, 18, 20),
            new ZoneData("outskirts", 10, 13, 16),
            new ZoneData("fun", 3, 9, 14),
            new ZoneData("docteur", 21)
        };

        internal static string FindZone(uint id) {
            foreach(var zoneData in ZoneForLevel) {
                if (zoneData.keys.Contains(id)) return zoneData.name;
            }

            return "";
        }
    }
}
using System;

namespace Archipelago.Data.Locations {
    enum LocationType : uint {
        Water = 0,
        Stickink = 1,
        Bouncink = 2,
        Splasher = 3,
        Clear = Splasher + Util.LevelCount * 7,
        Bronze = Clear + Util.LevelCount,
        Silver = Bronze + Util.LevelCount,
        Gold = Silver + Util.LevelCount,
        Platinum = Gold + Util.LevelCount
    }

    static class LocationExtensions {
        internal static LocationType? ToLocation(this Medal medal) {
            switch (medal) {
                case Medal.Bronze: return LocationType.Bronze;
                case Medal.Silver: return LocationType.Silver;
                case Medal.Gold: return LocationType.Gold;
                case Medal.Dev: return LocationType.Platinum;
                default: return null;
            }
        }

        internal static LocationType FindRange(long id) {
            var variants = Enum.GetValues(typeof(LocationType));
            var n = variants.Length - 1;

            for (uint i = 0; i < n; i++) {
                if (id < (uint)variants.GetValue(i + 1)) return (LocationType)variants.GetValue(i); 
            }

            return (LocationType)variants.GetValue(n);
        }
    }
}
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    abstract class Item {
        protected const string CATEGORY = "ArchipelagoItems";
        public abstract void Collect(ItemInfo item);
        public abstract string Name();
        public abstract bool SaveCollect();
        public abstract ItemFlags GetClassification();
    }
}
using System.Collections.Generic;
using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.Network.Items;

namespace Archipelago.Data.UI {
    internal static class Tracker {
        const string CATEGORY = "ArchipelagoTracker";
        const string PERSON_COLOR = "FA4DB2";
        private static readonly Queue<string> messages = new Queue<string>();

        private static string Color(ItemFlags classification) {
            switch(classification) {
                case ItemFlags.Advancement: return "FFD700";
                case ItemFlags.NeverExclude: return "9B83FC";
                case ItemFlags.None: return "4DFAF5";
                case ItemFlags.Trap: return "FA814D";
                default: return "FFFFFF";
            }
        }

        private static string ColorSpan(string text, string color) => $"<color=#{color}>{text}</color>";

        internal static string Get() {
            if (messages.Count == 0) return null;
            return messages.Dequeue();
        }

        internal static void AddItemReceived(Item item, ItemFlags classification, string sender=null) {
            var itemLabel = ColorSpan(item.Name(), Color(classification));
            messages.Enqueue(sender == null
                ? Language.Get(CATEGORY, "found", itemLabel)
                : Language.Get(CATEGORY, "received", itemLabel, ColorSpan(sender, PERSON_COLOR))
            );
        }

        internal static void AddItemSent(string item, ItemFlags classification, string receiver) {
            messages.Enqueue(Language.Get(
                CATEGORY, "sent", 
                ColorSpan(item, Color(classification)), ColorSpan(receiver, PERSON_COLOR)
            ));
        }
    }
}
using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using Archipelago.Network.Items;

namespace Archipelago.Data.UI {
    internal static class Tracker {
        private static readonly Queue<string> messages = new Queue<string>();

        private static string Color(Classification classification) {
            switch(classification) {
                case Classification.Progression: return "FFD700";
                case Classification.Useful: return "9B83FC";
                case Classification.Filler: return "4DFAF5";
                case Classification.Trap: return "FA814D";
                default: return "FFFFFF";
            }
        }

        private static string ColorSpan(string text, string color) => $"<color=#{color}>{text}</color>";

        internal static string Get() {
            if (messages.Count == 0) return null;
            return messages.Dequeue();
        }

        internal static void AddItemReceived(Item item, string sender=null) {
            var itemLabel = ColorSpan(item.Name(), Color(item.GetClassification()));
            messages.Enqueue(sender == null
                ? $"You found your {itemLabel} !"
                : $"Received {itemLabel} from {ColorSpan(sender, "FA4DB2")}"
            );
        }
    }
}
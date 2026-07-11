using System;
using System.Threading;

namespace Archipelago.Network.Helpers {
    internal class BackgroundThread {
        private readonly string name;
        private readonly Action action;
        private bool available = true;

        internal BackgroundThread(string name, Action action) {
            this.name = name;
            this.action = () => {
                action();
                available = true;
            };
        }

        internal void Execute() {
            if (!available) {
                Core.Static.Warn($"Aborted : {name} is busy.");
                return;
            }

            available = false;
            new Thread(new ThreadStart(action)) { IsBackground = true }.Start();
        } 
    }
}

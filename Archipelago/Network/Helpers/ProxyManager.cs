using System;
using System.Diagnostics;

namespace SplasherArchipelago.Network.Helpers {
    internal static class ProxyManager {
        private static Process _process = null;

        public static bool Init(Address address) {
            if (_process != null) {
                if (!_process.HasExited) return true;
                Clean();
            }

            return Start(address);
        }

        private static bool Start(Address address) {
            var process = new Process();
            process.StartInfo.FileName = "Proxy.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Arguments = $"{address.domain} {address.port}";
            process.EnableRaisingEvents = true;

            Util.Log($"Launching the proxy to listen on {address}");
            try {
                process.Start();
                _process = process;
                return true;
            } catch (Exception e) {
                Util.Error($"Failed to start the process.\nDetails: {e.Message}");
                return false;
            }
        }

        public static void Drop() {
            if (_process is null) return;

            _process.Close();
            Clean();
        }

        private static void Clean() {
            _process.Dispose();
            _process = null;
        }
    }
}

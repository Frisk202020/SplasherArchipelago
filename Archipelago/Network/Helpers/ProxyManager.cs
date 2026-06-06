using System;
using System.Diagnostics;
using System.Threading;

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
            process.StartInfo.Arguments = $"{address.domain} {address.port}";
            process.EnableRaisingEvents = true;
            process.StartInfo.UseShellExecute = false;

            Util.Log($"Launching the proxy to listen on {address}");
            try {
                process.Start();
                _process = process;
                Thread.Sleep(1000); // let proxy open its connection

                return true;
            } catch (Exception e) {
                Util.Error($"Failed to start the process.\nDetails: {e.Message}");
                return false;
            }
        }

        public static void Drop() {
            if (_process is null) return;

            try {
                _process.Kill();
                _process.WaitForExit();
            } catch (Exception) {
                Util.Error("Failed to terminate the proxy instance");
            } finally {
                Clean();
            }            
        }

        private static void Clean() {
            _process.Dispose();
            _process = null;
        }
    }
}

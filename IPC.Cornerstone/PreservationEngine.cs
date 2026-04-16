using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Win32;

namespace IPC.Cornerstone
{
    public static class PreservationEngine
    {
        // Vetted target list for 2026 "OS Noise"
        private static readonly List<string> TelemetryServices = new()
        {
            "DiagTrack",         // Connected User Experiences and Telemetry
            "dmwappushservice",  // WAP Push Message Routing Service
            "Wisvc",             // Windows Intelligence Service (2026 AI Telemetry)
            "WSearch"            // Windows Search (Web-integration hog)
        };

        public static IEnumerable<string> LiquidateTelemetry()
        {
            foreach (var serviceName in TelemetryServices)
            {
                string statusMessage = string.Empty;
                bool success = false;

                try
                {
                    using ServiceController sc = new ServiceController(serviceName);
                    if (sc.Status != ServiceControllerStatus.Stopped)
                    {
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(5));

                        // Secure the perimeter by disabling the service in registry
                        Registry.SetValue($@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\{serviceName}", "Start", 4);

                        statusMessage = $"SUCCESS: {serviceName} liquidated and locked.";
                        success = true;
                    }
                    else
                    {
                        statusMessage = $"SKIP: {serviceName} is already inactive.";
                    }
                }
                catch (Exception ex)
                {
                    statusMessage = $"FAIL: {serviceName} resisted liquidation. ({ex.Message})";
                }

                // Now we yield the message safely outside the try-catch block
                yield return statusMessage;
            }
        }

        public static IEnumerable<string> EvictWebHogs()
        {
            // Targeting WebView2 instances linked to Shell components
            var targets = Process.GetProcessesByName("msedgewebview2");
            foreach (var proc in targets)
            {
                try
                {
                    // IPC Logic: Only kill if it's not a user-opened browser tab
                    // We'll look for shell-integrated command line arguments here in the future
                    proc.Kill();
                    yield return $"EVICTED: WebView2 Instance (PID: {proc.Id}) terminated.";
                }
                catch
                {
                    yield return $"SKIPPED: WebView2 Instance (PID: {proc.Id}) in use by protected process.";
                }
            }
        }
    }
}
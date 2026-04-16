using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Linq;

namespace IPC.Cornerstone
{
    public static class PreservationEngine
    {
        // TARGETS: The "Deep State" of 25H2
        private static readonly string[] ExecutableBlacklist = {
            "CompatTelRunner.exe",   // Primary Telemetry Runner
            "DeviceCensus.exe",      // Hardware/Usage Telemetry
            "MicrosoftEdgeUpdate.exe",// Background Edge updater (bypasses standard controls)
            "mscorsvw.exe",          // .NET Optimization (Massive idle CPU Hog)
            "wsqmcons.exe"           // Windows SQM Consolidation (Data collection)
        };

        private static readonly string[] ServiceTargets = {
            "DiagTrack",        // Connected User Experiences and Telemetry
            "dmwappushservice", // WAP Push Message Routing
            "Wisvc",            // Windows Intelligence (AI Context Scanning)
            "WSearch",          // Windows Search (Web-integrated Indexer)
            "SysMain"           // Superfetch (Prefetches apps, heavy disk/RAM usage)
        };

        /// <summary>
        /// Executes Deep Liquidation. Pass in a logging method (e.g., AddLog) to see output.
        /// </summary>
        public static void ExecuteDeepProtocol(Action<string> log)
        {
            log("[PROTOCOL] INITIATING DEEP LIQUIDATION...");

            // ---------------------------------------------------------
            // STAGE 1: BLACK-HOLE EXECUTABLES (IFEO Debugger Trick)
            // This tells Windows to launch 'systray.exe' (which does nothing)
            // instead of the actual telemetry executable. Bypasses file permissions.
            // ---------------------------------------------------------
            log("[STAGE 1] Deploying Image File Execution Options (IFEO) Black-holes...");
            foreach (var exe in ExecutableBlacklist)
            {
                try
                {
                    using var key = Registry.LocalMachine.CreateSubKey($@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{exe}");
                    if (key != null)
                    {
                        // Reroutes the executable to a dummy lightweight process that exits silently
                        key.SetValue("Debugger", "systray.exe", RegistryValueKind.String);
                        log($"  -> ISOLATED: {exe} permanently redirected.");
                    }
                }
                catch (Exception ex)
                {
                    log($"  -> FAILED to isolate {exe}: {ex.Message}");
                }
            }

            // ---------------------------------------------------------
            // STAGE 2: WIPE SERVICE FAILURE ACTIONS (Prevent Self-Healing)
            // Microsoft sets services to "Restart" if they fail. Deleting 
            // 'FailureActions' permanently disables this zombie behavior.
            // ---------------------------------------------------------
            log("[STAGE 2] Neutralizing Services and Wiping Self-Healing Mechanisms...");
            foreach (var svc in ServiceTargets)
            {
                try
                {
                    // A. Stop the service immediately if running
                    using ServiceController sc = new ServiceController(svc);
                    if (sc.Status != ServiceControllerStatus.Stopped && sc.Status != ServiceControllerStatus.StopPending)
                    {
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(3));
                    }

                    // B. Hard-disable via Registry
                    using var svcKey = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{svc}", true);
                    if (svcKey != null)
                    {
                        svcKey.SetValue("Start", 4, RegistryValueKind.DWord);

                        // C. Destroy Failure Actions (The ultimate lock-down)
                        if (svcKey.GetValue("FailureActions") != null)
                        {
                            svcKey.DeleteValue("FailureActions");
                            log($"  -> SEVERED: {svc} recovery actions wiped.");
                        }
                        log($"  -> NEUTRALIZED: {svc} disabled at root level.");
                    }
                }
                catch (Exception ex)
                {
                    log($"  -> RESISTED: {svc} - {ex.Message}");
                }
            }

            // ---------------------------------------------------------
            // STAGE 3: EVICT ACTIVE WEB-HOGS & RECLAIM MEMORY
            // ---------------------------------------------------------
            log("[STAGE 3] Purging active WebView2 memory hogs...");
            int evictedCount = 0;
            long totalRamReclaimed = 0;

            foreach (var proc in Process.GetProcessesByName("msedgewebview2"))
            {
                try
                {
                    totalRamReclaimed += proc.WorkingSet64;
                    proc.Kill();
                    proc.WaitForExit(1000);
                    evictedCount++;
                }
                catch { /* Ignore protected or user-owned Edge instances */ }
            }

            double mbReclaimed = Math.Round(totalRamReclaimed / 1024.0 / 1024.0, 2);
            log($"  -> EVICTED {evictedCount} WebView2 instances.");
            log($"  -> RECLAIMED {mbReclaimed} MB of RAM.");

            // ---------------------------------------------------------
            // STAGE 4: DECOUPLE WEB SEARCH
            // ---------------------------------------------------------
            log("[STAGE 4] Decoupling Web Search from Shell...");
            try
            {
                const string searchPath = @"Software\Microsoft\Windows\CurrentVersion\Search";
                using var hkcuSearch = Registry.CurrentUser.CreateSubKey(searchPath);
                if (hkcuSearch != null)
                {
                    hkcuSearch.SetValue("BingSearchEnabled", 0, RegistryValueKind.DWord);
                    hkcuSearch.SetValue("SearchboxTaskbarMode", 0, RegistryValueKind.DWord);
                    hkcuSearch.SetValue("ConnectedSearchUseWeb", 0, RegistryValueKind.DWord);
                    log("  -> DECOUPLED: Start Menu is now strictly local.");
                }
            }
            catch (Exception ex)
            {
                log($"  -> FAILED Search Decoupling: {ex.Message}");
            }

            log("[PROTOCOL] SYSTEM INTEGRITY RESTORED.");
        }
    }
}
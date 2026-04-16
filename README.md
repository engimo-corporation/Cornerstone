# IPC // CORNERSTONE

### Project Preservation Protocol

> "Dismantling overreach to restore consumer sovereignty. We liquidate the noise so the machine can breathe."

**Cornerstone** is a surgical system utility built on the **Interastral Peace Corporation (IPC)** framework. It is designed to reclaim system resources from persistent "OS Noise"—including telemetry services, web-based shell components (WebView2), and cloud-integrated search—specifically for the 95% of consumers running Windows Home editions.

## 🛠 Engineering Specifications

Cornerstone is developed with a focus on native performance and "Silent Architect" stability.

* **Integrated Development Environment**: Visual Studio 2026 Community (Version 18.5+)

* **Core Framework**: .NET 8.0 LTS (Long-Term Support)

* **UI Architecture**: WinUI 3 (Windows App SDK)

* **Language**: C# 14.0

* **Distribution**: Portable x64 Native

## 🎯 The Mission: The Consumer Baseline

Most system tools rely on Group Policy (GPO) or Enterprise-only registry hives that the Windows Home edition ignores. Cornerstone is engineered to bypass these limitations by targeting the actual service states and user-level hives that define the modern Windows experience.

**Primary Objectives:**

* **Surgical Liquidation**: Terminating and hard-locking telemetry services like `DiagTrack` and `Wisvc`.

* **Search Decoupling**: Severing the Start Menu's connection to Bing/Cloud search to restore 100% local, low-latency indexing.

* **Shell Eviction**: Identifying and neutralizing non-essential `msedgewebview2` instances tied to background shell components (Widgets, News, etc.).

## 🧪 Verification & Testing Protocol

To ensure deterministic reliability, Cornerstone is vetted in a hardened "Clean Room" environment.

* **Hypervisor**: VMware Workstation Pro 25H2u1

* **Target OS**: Windows 11 Home (Version 25H2)

* **Testing Rig**: 
    * RAM: 4.00 GB (Emulating the standard consumer baseline)
    * **TPM: Virtual TPM 2.0 Enabled
    * **Secure Boot**: Enabled

### Testing Methodology:

1. Baseline Generation: A fresh "Vanilla" install of Windows 11 Home is snapshotted (`STABLE_BLOAT_BASELINE`).
2. Execution: Cornerstone is deployed to liquidate background telemetry and web-hogs.
3. Verification: Success is measured by a permanent reduction in idle RAM usage and the absence of web-based results in the Start Menu.

## 🏛 The IPC Philosophy

In a digital landscape dominated by "Subscriptions as a Service" and pervasive telemetry, Cornerstone stands as a Sovereign Standard. We believe that integrity is a right, not a privilege.

* Zero Telemetry: Cornerstone does not phone home.

* Local-First: All operations are performed on the machine, for the machine.

* No Subscriptions: Built by the IPC for the dignity of the consumer.

##📜 License

Distributed under the [MIT License](LICENSE).

*"We will tear the overreacher's bit by bit."*
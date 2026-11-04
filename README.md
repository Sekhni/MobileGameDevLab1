# Pixel Sprint (Endless Runner)

Mobile Game Development (A12581). Unity 6.6 (6000.6.0f1), Android, sideloaded APK.

## Device
- Phone: samsung SM-S901B
- Android: OS 16 (API 36)
- Graphics API: Vulkan

## Build profiles
| Profile | Development Build | Autoconnect Profiler | Used for |
|---|---|---|---|
| Android Dev | on | on | Profiling and debugging (Probe button visible) |
| Android Release | off | off | Release-signed APK for hand-in and measurements |

## Signing
- Keystore: `C:\Users\ahmad\user.keystore` (outside the repo; `*.keystore` and `*.jks` are in `.gitignore`)
- Alias: `moblie game dev`
- Validity: 16 Sep 2026 to 3 Sep 2076 (50 years); certificate owner O=DefaultCompany
- SHA-256 fingerprint: `54:18:BC:47:9B:D5:51:95:41:30:13:31:D5:C8:E9:18:7E:28:25:2D:DC:2B:F0:ED:A1:04:7B:C5:68:D0:7B:C2`
- Note: keytool reports the certificate signature as SHA1withRSA (flagged weak). Fine for sideloading; Play App Signing re-signs uploads.
- Backups: two copies (password manager attachment + encrypted cloud/drive). Passwords are kept only in the password manager, never in this repo.

## Build (clone to phone)
1. Clone the repo: `git clone <repo-url>` and open the folder in **Unity 6.6 (6000.6.0f1)** with Android Build Support installed.
2. **File > Build Profiles**: select **Android Release** and click **Switch Profile** (Development Build off, Build App Bundle off).
3. **Edit > Project Settings > Player > Android** checklist:
   - Package Name `com.ahmedsekhni.mobilegamedev` (never changes)
   - Version `0.2.0`, Bundle Version Code `2`
   - Minimum API Level 26, Target API Level Automatic (highest installed)
   - Scripting Backend IL2CPP, Target Architectures ARM64
   - Publishing Settings: Custom Keystore ticked, keystore + alias selected, both passwords typed (Unity does not save them between sessions)
4. Click **Build** and save as `releases/MyGame-0.2.0-arm64.apk`.
5. Install and check the version:
   ```
   adb install -r releases/MyGame-0.2.0-arm64.apk
   adb shell dumpsys package com.ahmedsekhni.mobilegamedev | findstr version
   ```
   Expect `Success`, `versionCode=2`, `versionName=0.2.0`. If an older debug-signed build is on the phone you get `INSTALL_FAILED_UPDATE_INCOMPATIBLE`: run `adb uninstall com.ahmedsekhni.mobilegamedev` and install again.
6. Launch: `adb shell monkey -p com.ahmedsekhni.mobilegamedev 1`, logs with `adb logcat -s Unity`.

Release builds have no INTERNET permission (`Assets/Scripts/Editor/StripInternetPermission.cs`) and Unity Diagnostics is disabled; see `docs/CA1/privacy-statement.md`.

Helper: `Builds\w04-release.ps1` does steps 5–6 and saves the output (Builds/ is not tracked).

## CA1 pack
`docs/CA1/`: install-proof.txt, device-screenshot.png, device-matrix.md, store-assets-checklist.md, descriptions.md, privacy-statement.md, mda-onepager.md. Dev journal: `docs/dev-journal.md`. APK: `releases/MyGame-0.2.0-arm64.apk`. Tag: `v0.2.0-ca1`.

## Device targets
- Minimum API level: 26 (Android 8.0)
- Target API level: Automatic. Play's current target-API requirement: https://support.google.com/googleplay/android-developer/answer/11926878
- 64-bit: ARM64 only (IL2CPP)
- Tested devices: see [docs/CA1/device-matrix.md](docs/CA1/device-matrix.md)

## Versioning note
Week 3 test builds used 0.1.1 / versionCode 3. For this lab the release is 0.2.0 / versionCode 2 as the lab sheet specifies; it installs because the switch from the debug key to the release key required uninstalling the old build first. Every later release raises versionCode from 2.

## AI assistance
Claude (Anthropic) was used during Weeks 2–4 for:  drafting the CA1 store checklist, listing copy and privacy statement from my build evidence; debugging the lane-change input, editor setup scripts (Week3Setup, Week4Setup, UILayoutFix), the UI layout fix, profiling-session help and helper PowerShell scripts for adb measurements. Measurements were taken on my own device; I reviewed all code and notes.

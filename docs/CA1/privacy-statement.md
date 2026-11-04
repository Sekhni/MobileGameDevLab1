# Data-use statement (CA1): Pixel Sprint

Applies to `com.ahmedsekhni.mobilegamedev` version 0.2.0 (versionCode 2), the release APK in `releases/`. Written from the evidence below, not from plans for later versions.

## Data collected

**None.** The game does not collect, transmit or share any personal or device data. It has no account, no analytics, no ads, no crash reporting and no online features. Unity's engine Diagnostics (Developer Data) is switched off for the Android release build, so the Unity runtime does not send crash or device reports either.

## Data stored on the device

Only two settings, saved with Unity `PlayerPrefs` in the app's private storage on the phone:

| Key | Value | Written by |
|---|---|---|
| `haptics` | 1 or 0 (vibration on or off) | Settings > Haptics toggle (`Haptics.cs`) |
| `textScale` | 0.85, 1.0 or 1.25 | Settings > Small / Normal / Large (`TextScale.cs`) |

Planned for CA2 (MDA): the single best score will be added as one more `PlayerPrefs` key on the phone. There are no save files and nothing is written to `Application.persistentDataPath` in this version. The Week 6 telemetry log will be written there and will stay local only (never uploaded). Nobody else can read this data. A player removes all of it by uninstalling the game, or with Settings > Apps > [game] > Storage > Clear storage.

## Network activity

**None.** The game makes no network requests: no `UnityWebRequest`, sockets, ads, analytics or services calls exist in the project's scripts, and the APK does not request the INTERNET permission (see below).

How that was made true: the first 0.2.0 build listed INTERNET. Two causes were fixed before release: Unity's engine Diagnostics was on (it sends crash and device reports to Unity), so it is now Disabled in Project Settings > Services > Diagnostics; and Unity's Internet Access "Auto" adds INTERNET whenever its networking modules are linked, so the editor script `Assets/Scripts/Editor/StripInternetPermission.cs` removes it from release builds only (Development builds keep it because the Profiler connects over a local socket). The rebuilt APK was re-checked with dumpsys.

## Third-party SDKs

None this semester. The build contains only the Unity runtime and Unity's own packages (Input System, UI/TextMeshPro, URP, AndroidX startup). No ads, analytics, social or crash-reporting SDKs.

## Permissions requested

From `adb shell dumpsys package com.ahmedsekhni.mobilegamedev` on the installed release build (Samsung SM-S901B, Android 16, 2026-11-04 18:20):

```
requested permissions:
  com.ahmedsekhni.mobilegamedev.DYNAMIC_RECEIVER_NOT_EXPORTED_PERMISSION
  android.permission.VIBRATE
install permissions:
  com.ahmedsekhni.mobilegamedev.DYNAMIC_RECEIVER_NOT_EXPORTED_PERMISSION: granted=true
  android.permission.VIBRATE: granted=true
```

| Permission | Why |
|---|---|
| `android.permission.VIBRATE` | One short haptic pulse (`Handheld.Vibrate`), only when the Haptics setting is on. In 0.2.0 it fires on a lane change; the MDA moves it to the crash. |
| `com.ahmedsekhni.mobilegamedev.DYNAMIC_RECEIVER_NOT_EXPORTED_PERMISSION` | Added automatically by AndroidX for apps targeting API 33+; it is private to this app, protects its internal broadcast receivers and gives the app no access to user data. |

## How this would be declared on Google Play

| Statement heading | Data safety form | Answer |
|---|---|---|
| Data collected | Data collection: "Does your app collect or share any of the required user data types?" | No. ("Collect" means sending data off the device; settings that stay on the phone are not collection.) |
| Data stored on the device | Not declared: on-device-only data is out of scope for the form | Kept in the privacy policy for transparency |
| Network activity / Third-party SDKs | Data sharing (including data sent by libraries and SDKs) | No data shared with third parties |
| Data collected | Security practices: encryption in transit, data deletion request | Not applicable, since nothing is collected or transmitted |
| This page | Privacy policy URL (App content) | Required even when nothing is collected; this statement would be published at a public URL |

The Data safety form must be completed before release on the closed, open or production tracks; an app that is active only on the internal testing track is exempt. Sources: Play Console Help, "Provide information for Google Play's Data safety section" (https://support.google.com/googleplay/android-developer/answer/10787469), read 2026-11-04.

## Checked against the build

- [x] Permission list above pasted from dumpsys on the installed 0.2.0 release build
- [x] INTERNET is not in that list (matches "Network activity: none")
- [x] VIBRATE is in the list and is used (Haptics toggle)
- [x] Engine Diagnostics disabled for the build (Build Profiles > Android Release > Diagnostics Data: Disabled)

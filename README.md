# Endless Runner — Option 

## Device
- Phone: samsung SM-S901B
- Android: OS 16 (API 36)
- Graphics API: Vulkan

## Build steps
1. Build in Unity: File > Build Profiles > Android > Build
2. Install: `adb install -r Builds/MyGame-dev.apk`
3. Run + log: `adb shell monkey -p com.ahmedsekhni.mobilegamedev 1` then `adb logcat -s Unity`
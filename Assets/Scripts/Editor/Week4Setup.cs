using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

// Tools > Week 4 > ...  Release-pipeline helpers for W04 Lab A.
public static class Week4Setup
{
    // Part A: identification and configuration exactly as the lab sheet lists them.
    [MenuItem("Tools/Week 4/Lab A Settings")]
    static void LabASettings()
    {
        PlayerSettings.bundleVersion = "0.2.0";                       // versionName
        PlayerSettings.Android.bundleVersionCode = 2;                 // versionCode (lab value)
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        EditorUserBuildSettings.buildAppBundle = false;               // CA1 submits an APK
        AssetDatabase.SaveAssets();

        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "releases"));
        Report();
    }

    [MenuItem("Tools/Week 4/Report Release Settings")]
    static void Report()
    {
        string ks = PlayerSettings.Android.useCustomKeystore
            ? $"custom keystore '{PlayerSettings.Android.keystoreName}', alias '{PlayerSettings.Android.keyaliasName}'"
            : "NO custom keystore (debug-signed)";
        Debug.Log("[Week4] " +
            $"package {PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.Android)} | " +
            $"version {PlayerSettings.bundleVersion} ({PlayerSettings.Android.bundleVersionCode}) | " +
            $"minSdk {PlayerSettings.Android.minSdkVersion} | targetSdk {PlayerSettings.Android.targetSdkVersion} | " +
            $"backend {PlayerSettings.GetScriptingBackend(NamedBuildTarget.Android)} | " +
            $"arch {PlayerSettings.Android.targetArchitectures} | appBundle {EditorUserBuildSettings.buildAppBundle} | {ks}");
    }
}

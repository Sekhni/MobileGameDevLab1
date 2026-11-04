using System.IO;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;

// Release builds only: remove android.permission.INTERNET from the merged manifest.
// The game makes no network calls (no UnityWebRequest, sockets, ads, analytics, services),
// but Unity's "Internet Access: Auto" adds INTERNET because engine modules that can use
// the network (UnityWebRequest, Analytics, Connect) are linked in.
// Development builds keep it: the Profiler and Frame Debugger connect over a local socket.
// See docs/CA1/privacy-statement.md.
class StripInternetPermission : IPostGenerateGradleAndroidProject
{
    public int callbackOrder => 1000;

    const string Permission = "android.permission.INTERNET";

    public void OnPostGenerateGradleAndroidProject(string unityLibraryPath)
    {
        if (EditorUserBuildSettings.development) return;

        // 1. Drop the line Unity wrote into unityLibrary's manifest.
        string libManifest = Path.Combine(unityLibraryPath, "src", "main", "AndroidManifest.xml");
        if (File.Exists(libManifest))
        {
            var lines = File.ReadAllLines(libManifest);
            var kept = System.Array.FindAll(lines, l => !(l.Contains("uses-permission") && l.Contains(Permission)));
            File.WriteAllLines(libManifest, kept);
        }

        // 2. Tell the Gradle manifest merger to remove it from every dependency too.
        string launcherManifest = Path.Combine(unityLibraryPath, "..", "launcher", "src", "main", "AndroidManifest.xml");
        if (File.Exists(launcherManifest))
        {
            string xml = File.ReadAllText(launcherManifest);
            if (!xml.Contains("xmlns:tools"))
                xml = xml.Replace("<manifest ", "<manifest xmlns:tools=\"http://schemas.android.com/tools\" ");
            if (!xml.Contains(Permission))
            {
                int i = xml.IndexOf('>', xml.IndexOf("<manifest")) + 1;
                xml = xml.Insert(i, "\n  <uses-permission android:name=\"" + Permission + "\" tools:node=\"remove\" />");
            }
            File.WriteAllText(launcherManifest, xml);
        }
        Debug.Log("[Release] INTERNET permission removed from the Android manifest (release build).");
    }
}

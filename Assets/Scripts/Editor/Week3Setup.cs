using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// Tools > Week 3 > ...  One-click scene setup for W03 Labs A and B.
// Editor-only: lives in an Editor folder, so it is never in a build.
public static class Week3Setup
{
    const string ButtonName = "ProbeButton";
    const string StressButtonName = "StressButton";

    [MenuItem("Tools/Week 3/Add Probe Button")]
    static void AddProbeButton()
    {
        var safeArea = Object.FindAnyObjectByType<SafeArea>(FindObjectsInactive.Include);
        if (safeArea == null)
        {
            EditorUtility.DisplayDialog("Week 3", "No SafeArea panel found in the open scene.", "OK");
            return;
        }
        var existing = safeArea.transform.Find(ButtonName);
        if (existing != null)
        {
            Selection.activeGameObject = existing.gameObject;
            EditorUtility.DisplayDialog("Week 3", "ProbeButton already exists under the SafeArea panel.", "OK");
            return;
        }

        // Button in the bottom-left corner of the SafeArea panel.
        var go = CreateButton(safeArea.transform, ButtonName, "Probe", new Vector2(0f, 0f), new Vector2(24f, 24f));

        // Probe + debug-only visibility, and OnClick -> RenderScaleProbe.Toggle.
        var probe = go.AddComponent<RenderScaleProbe>();
        probe.lowScale = 0.4f;   // Mobile_RPAsset Render Scale is 0.8, so 0.4 is half
        go.AddComponent<DebugBuildOnly>();
        UnityEventTools.AddPersistentListener(go.GetComponent<Button>().onClick, probe.Toggle);

        Undo.RegisterCreatedObjectUndo(go, "Add Probe Button");
        EditorSceneManager.MarkSceneDirty(go.scene);
        Selection.activeGameObject = go;
        Debug.Log("[Week3] ProbeButton added under " + safeArea.name + ". Save the scene (Ctrl+S).");
    }

    [MenuItem("Tools/Week 3/Add 200-Cube Stress Test")]
    static void AddStressCubes()
    {
        if (Object.FindAnyObjectByType<StressCubes>(FindObjectsInactive.Include) != null)
        {
            EditorUtility.DisplayDialog("Week 3", "A StressCubes object is already in the scene.", "OK");
            return;
        }
        var go = new GameObject("StressCubes (W03)", typeof(StressCubes));
        var cam = Camera.main;
        if (cam != null)
        {
            Vector3 fwd = cam.transform.forward; fwd.y = 0f;
            go.transform.position = cam.transform.position + fwd.normalized * 8f + Vector3.down * 1f;
        }
        Undo.RegisterCreatedObjectUndo(go, "Add Stress Cubes");
        EditorSceneManager.MarkSceneDirty(go.scene);
        Selection.activeGameObject = go;
        Debug.Log("[Week3] StressCubes added. Save the scene (Ctrl+S). Disable it after the lab.");
    }

    // Lab B: sampler on Bootstrap, a Stress on/off button (works in release
    // builds too), version 0.1.1, Optimized Frame Pacing on, then save.
    [MenuItem("Tools/Week 3/Lab B Setup")]
    static void LabBSetup()
    {
        var log = new System.Text.StringBuilder();

        // 1. FrameTimeSampler on the Bootstrap object (first and only scene).
        var boot = Object.FindAnyObjectByType<MobileBootstrap>(FindObjectsInactive.Include);
        if (boot == null) { EditorUtility.DisplayDialog("Week 3", "No MobileBootstrap found in the open scene.", "OK"); return; }
        if (boot.GetComponent<FrameTimeSampler>() == null)
        {
            Undo.AddComponent<FrameTimeSampler>(boot.gameObject);
            log.AppendLine("Added FrameTimeSampler to " + boot.name);
        }
        else log.AppendLine("FrameTimeSampler already on " + boot.name);

        // 2. Stress button, bottom-right of the SafeArea panel, bound to StressCubes.Toggle.
        var safeArea = Object.FindAnyObjectByType<SafeArea>(FindObjectsInactive.Include);
        var cubes = Object.FindAnyObjectByType<StressCubes>(FindObjectsInactive.Include);
        if (safeArea != null && cubes != null && safeArea.transform.Find(StressButtonName) == null)
        {
            var go = CreateButton(safeArea.transform, StressButtonName, "Stress", new Vector2(1f, 0f), new Vector2(-24f, 24f));
            UnityEventTools.AddPersistentListener(go.GetComponent<Button>().onClick, cubes.Toggle);
            Undo.RegisterCreatedObjectUndo(go, "Add Stress Button");
            log.AppendLine("Added Stress button (bottom-right)");
        }
        else if (cubes == null) log.AppendLine("No StressCubes in scene: run Add 200-Cube Stress Test first");
        else log.AppendLine("Stress button already present");

        // 3. Player settings for the release build.
        PlayerSettings.bundleVersion = "0.1.1";
        if (PlayerSettings.Android.bundleVersionCode < 3) PlayerSettings.Android.bundleVersionCode = 3;
        PlayerSettings.Android.optimizedFramePacing = true;
        log.AppendLine($"Version {PlayerSettings.bundleVersion} ({PlayerSettings.Android.bundleVersionCode}), Optimized Frame Pacing on");

        EditorSceneManager.MarkSceneDirty(boot.gameObject.scene);
        EditorSceneManager.SaveScene(boot.gameObject.scene);
        AssetDatabase.SaveAssets();
        log.Append("Scene and project saved.");
        Debug.Log("[Week3] Lab B setup:\n" + log);
    }

    static GameObject CreateButton(Transform parent, string name, string text, Vector2 corner, Vector2 offset)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        go.layer = LayerMask.NameToLayer("UI");
        var rt = (RectTransform)go.transform;
        rt.SetParent(parent, false);
        rt.anchorMin = rt.anchorMax = rt.pivot = corner;
        rt.anchoredPosition = offset;
        rt.sizeDelta = new Vector2(180f, 80f);
        go.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.6f);

        var label = new GameObject("Text (TMP)", typeof(RectTransform), typeof(TextMeshProUGUI));
        label.layer = go.layer;
        var lrt = (RectTransform)label.transform;
        lrt.SetParent(rt, false);
        lrt.anchorMin = Vector2.zero; lrt.anchorMax = Vector2.one;
        lrt.offsetMin = lrt.offsetMax = Vector2.zero;
        var tmp = label.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 36;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        return go;
    }
}

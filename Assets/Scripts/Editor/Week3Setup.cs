using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// Tools > Week 3 > ...  One-click scene setup for W03 Lab A.
// Editor-only: lives in an Editor folder, so it is never in a build.
public static class Week3Setup
{
    const string ButtonName = "ProbeButton";

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
        var go = new GameObject(ButtonName, typeof(RectTransform), typeof(Image), typeof(Button));
        go.layer = LayerMask.NameToLayer("UI");
        var rt = (RectTransform)go.transform;
        rt.SetParent(safeArea.transform, false);
        rt.anchorMin = rt.anchorMax = rt.pivot = Vector2.zero;
        rt.anchoredPosition = new Vector2(24f, 24f);
        rt.sizeDelta = new Vector2(180f, 80f);
        go.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.6f);

        var label = new GameObject("Text (TMP)", typeof(RectTransform), typeof(TextMeshProUGUI));
        label.layer = go.layer;
        var lrt = (RectTransform)label.transform;
        lrt.SetParent(rt, false);
        lrt.anchorMin = Vector2.zero; lrt.anchorMax = Vector2.one;
        lrt.offsetMin = lrt.offsetMax = Vector2.zero;
        var tmp = label.GetComponent<TextMeshProUGUI>();
        tmp.text = "Probe";
        tmp.fontSize = 36;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;

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
}

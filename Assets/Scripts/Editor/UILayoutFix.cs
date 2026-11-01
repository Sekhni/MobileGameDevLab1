using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// Tools > Week 3 > Fix UI Layout
// One pass that tidies the HUD, pause menu and settings panel in the open scene.
// Sizes are in Canvas reference pixels (1080x1920). 150 px is about 55 dp on a
// 1080x2340 phone, so every button clears the 48 dp touch-target minimum.
// Everything goes through Undo, so Ctrl+Z reverts it.
public static class UILayoutFix
{
    static readonly Color PanelDim   = new Color(0.04f, 0.06f, 0.10f, 0.88f);
    static readonly Color PanelSolid = new Color(0.08f, 0.09f, 0.13f, 1f);
    static readonly Color ButtonFace = new Color(0.95f, 0.96f, 0.98f, 1f);
    static readonly Color ButtonText = new Color(0.10f, 0.11f, 0.14f, 1f);
    static readonly Color LightText  = Color.white;
    static readonly Color SubText    = new Color(0.80f, 0.83f, 0.90f, 1f);
    static readonly Color Accent     = new Color(0.18f, 0.62f, 0.38f, 1f);

    static readonly Vector2 Center = new Vector2(0.5f, 0.5f);

    [MenuItem("Tools/Week 3/Fix UI Layout")]
    static void Fix()
    {
        var safe = Object.FindAnyObjectByType<SafeArea>(FindObjectsInactive.Include);
        var menu = Object.FindAnyObjectByType<PauseMenu>(FindObjectsInactive.Include);
        if (safe == null || menu == null)
        {
            EditorUtility.DisplayDialog("Fix UI Layout", "Needs a SafeArea panel and a PauseMenu in the open scene.", "OK");
            return;
        }
        Undo.SetCurrentGroupName("Fix UI Layout");
        int group = Undo.GetCurrentGroup();

        // 1. SafeArea: stretch, zero offsets, scale 1 (it had 1.0 x 0.95 x 0.47).
        var sa = (RectTransform)safe.transform;
        Undo.RecordObject(sa, "SafeArea");
        sa.anchorMin = Vector2.zero; sa.anchorMax = Vector2.one;
        sa.offsetMin = Vector2.zero; sa.offsetMax = Vector2.zero;
        sa.localScale = Vector3.one;

        var pause = sa.Find("Pause Panel");
        var settings = pause != null ? pause.Find("SettingsPanel") : null;
        if (pause == null || settings == null)
        {
            EditorUtility.DisplayDialog("Fix UI Layout", "Expected 'Pause Panel/SettingsPanel' under the SafeArea panel.", "OK");
            return;
        }

        // 2. Pause panel: dark overlay, centred title, two big buttons.
        Stretch(pause); SetPanel(pause, PanelDim);
        Place(pause.Find("Pause"), new Vector2(0, 420), new Vector2(900, 170));
        StyleText(pause.Find("Pause"), "Paused", 96, LightText, TextAlignmentOptions.Center);
        Place(pause.Find("Resume"), new Vector2(0, 150), new Vector2(640, 160));
        StyleButton(pause.Find("Resume"), "Resume", 60);
        Place(pause.Find("Settings"), new Vector2(0, -50), new Vector2(640, 160));
        StyleButton(pause.Find("Settings"), "Settings", 60);
        pause.SetAsLastSibling();   // draw above the HUD and debug buttons

        // 3. Settings panel: title, text-size row, haptics toggle, back at the bottom.
        Stretch(settings); SetPanel(settings, PanelSolid);
        var title = Label(settings, "Title", "Settings", 84, LightText, new Vector2(0, 620), new Vector2(900, 150));
        title.SetAsFirstSibling();
        Label(settings, "TextSizeLabel", "Text size", 52, SubText, new Vector2(0, 410), new Vector2(900, 90));
        Place(settings.Find("Small"), new Vector2(-320, 260), new Vector2(290, 150));
        StyleButton(settings.Find("Small"), "Small", 52);
        Place(settings.Find("Normal"), new Vector2(0, 260), new Vector2(290, 150));
        StyleButton(settings.Find("Normal"), "Normal", 52);
        Place(settings.Find("Large"), new Vector2(320, 260), new Vector2(290, 150));
        StyleButton(settings.Find("Large"), "Large", 52);
        FixHapticsToggle(settings.Find("HapticsToggle"));
        Place(settings.Find("BackButton"), new Vector2(0, -320), new Vector2(640, 160));
        StyleButton(settings.Find("BackButton"), "Back", 60);

        // 4. HUD pause button, top-right inside the safe area.
        var pb = sa.Find("PauseButton");
        if (pb == null)
        {
            var go = new GameObject("PauseButton", typeof(RectTransform), typeof(Image), typeof(Button));
            go.layer = LayerMask.NameToLayer("UI");
            Undo.RegisterCreatedObjectUndo(go, "PauseButton");
            go.transform.SetParent(sa, false);
            pb = go.transform;
            var label = Label(pb, "Text (TMP)", "II", 64, LightText, Vector2.zero, Vector2.zero);
            var lrt = (RectTransform)label;
            lrt.anchorMin = Vector2.zero; lrt.anchorMax = Vector2.one;
            lrt.offsetMin = lrt.offsetMax = Vector2.zero;
            UnityEventTools.AddPersistentListener(go.GetComponent<Button>().onClick, menu.OnPausePressed);
        }
        var prt = (RectTransform)pb;
        Undo.RecordObject(prt, "PauseButton");
        prt.anchorMin = prt.anchorMax = prt.pivot = new Vector2(1f, 1f);
        prt.anchoredPosition = new Vector2(-32f, -32f);
        prt.sizeDelta = new Vector2(150f, 150f);
        prt.localScale = Vector3.one;
        SetImage(pb, new Color(0f, 0f, 0f, 0.55f));
        pause.SetAsLastSibling();

        EditorSceneManager.MarkSceneDirty(sa.gameObject.scene);
        EditorSceneManager.SaveScene(sa.gameObject.scene);
        Undo.CollapseUndoOperations(group);
        Debug.Log("[UI] Layout fixed and scene saved: SafeArea scale reset, pause/settings rebuilt, Pause button added.");
    }

    static void FixHapticsToggle(Transform t)
    {
        if (t == null) return;
        Place(t, new Vector2(0, 40), new Vector2(640, 150));
        var bg = t.Find("Background") as RectTransform;
        if (bg != null)
        {
            Undo.RecordObject(bg, "Toggle bg");
            bg.anchorMin = bg.anchorMax = bg.pivot = new Vector2(0f, 0.5f);
            bg.anchoredPosition = new Vector2(24f, 0f);
            bg.sizeDelta = new Vector2(100f, 100f);
            SetImage(bg, ButtonFace);
            var check = bg.Find("Checkmark") as RectTransform;
            if (check != null)
            {
                Undo.RecordObject(check, "Checkmark");
                check.anchorMin = check.anchorMax = check.pivot = Center;
                check.anchoredPosition = Vector2.zero;
                check.sizeDelta = new Vector2(76f, 76f);
                SetImage(check, Accent);
            }
        }
        // The old legacy-Text label is not scaled by TextScale: hide it and use the TMP text.
        var legacy = t.Find("Label");
        if (legacy != null) { Undo.RecordObject(legacy.gameObject, "Label"); legacy.gameObject.SetActive(false); }
        var tmp = t.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            Undo.RecordObject(tmp, "Haptics text");
            tmp.text = "Haptics";
            tmp.enableAutoSizing = false;
            tmp.fontSize = 56;
            tmp.color = LightText;
            tmp.alignment = TextAlignmentOptions.MidlineLeft;
            tmp.margin = new Vector4(160f, 0f, 0f, 0f);
            tmp.raycastTarget = false;
        }
    }

    static void Stretch(Transform t)
    {
        var rt = (RectTransform)t;
        Undo.RecordObject(rt, "Stretch");
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;
    }

    static void Place(Transform t, Vector2 pos, Vector2 size)
    {
        if (t == null) return;
        var rt = (RectTransform)t;
        Undo.RecordObject(rt, "Place");
        rt.anchorMin = rt.anchorMax = rt.pivot = Center;
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
        rt.localScale = Vector3.one;   // buttons were 160x30 at scale 2
    }

    // Full-screen panels: plain colour, no sprite. The built-in 'Background'
    // sprite is partly transparent, so the pause menu showed through Settings.
    static void SetPanel(Transform t, Color c)
    {
        var img = t.GetComponent<Image>();
        if (img == null) return;
        Undo.RecordObject(img, "Panel");
        img.sprite = null;
        img.type = Image.Type.Simple;
        img.color = c;
    }

    static void SetImage(Transform t, Color c)
    {
        var img = t.GetComponent<Image>();
        if (img == null) return;
        Undo.RecordObject(img, "Image");
        img.color = c;
    }

    static void StyleButton(Transform t, string text, float size)
    {
        if (t == null) return;
        SetImage(t, ButtonFace);
        var tmp = t.GetComponentInChildren<TextMeshProUGUI>(true);
        if (tmp != null) StyleText(tmp.transform, text, size, ButtonText, TextAlignmentOptions.Center);
    }

    static void StyleText(Transform t, string text, float size, Color color, TextAlignmentOptions align)
    {
        if (t == null) return;
        var tmp = t.GetComponent<TextMeshProUGUI>();
        if (tmp == null) return;
        Undo.RecordObject(tmp, "Text");
        tmp.text = text;
        tmp.enableAutoSizing = false;   // Auto Size overrides TextScale (Week 2 Lab B, Part C)
        tmp.fontSize = size;
        tmp.color = color;
        tmp.alignment = align;
        tmp.margin = Vector4.zero;      // the old title had a -351 px right margin
        tmp.overflowMode = TextOverflowModes.Ellipsis;
        tmp.textWrappingMode = TextWrappingModes.NoWrap;
        if (t.GetComponent<TextScale>() == null) Undo.AddComponent<TextScale>(t.gameObject);
    }

    static Transform Label(Transform parent, string name, string text, float size, Color color, Vector2 pos, Vector2 box)
    {
        var t = parent.Find(name);
        if (t == null)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
            go.layer = LayerMask.NameToLayer("UI");
            Undo.RegisterCreatedObjectUndo(go, name);
            go.transform.SetParent(parent, false);
            t = go.transform;
        }
        if (box != Vector2.zero) Place(t, pos, box);
        StyleText(t, text, size, color, TextAlignmentOptions.Center);
        t.GetComponent<TextMeshProUGUI>().raycastTarget = false;
        return t;
    }
}

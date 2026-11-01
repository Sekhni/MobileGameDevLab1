using UnityEngine;
using UnityEngine.UI;

// Binds the Settings toggle to Haptics.Enabled (stored in PlayerPrefs).
[RequireComponent(typeof(Toggle))]
public class HapticsToggle : MonoBehaviour
{
    Toggle _toggle;

    void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.SetIsOnWithoutNotify(Haptics.Enabled);
        _toggle.onValueChanged.AddListener(OnChanged);
    }

    void OnEnable()
    {
        if (_toggle != null) _toggle.SetIsOnWithoutNotify(Haptics.Enabled);
    }

    void OnDestroy()
    {
        if (_toggle != null) _toggle.onValueChanged.RemoveListener(OnChanged);
    }

    void OnChanged(bool on)
    {
        Haptics.Enabled = on;
        if (on) Haptics.Pulse();   // a short buzz confirms the setting
    }
}

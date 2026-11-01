using UnityEngine;

// Hides this GameObject in release builds. Debug.isDebugBuild is true in the
// Editor and in Development Builds, false in release builds.
public class DebugBuildOnly : MonoBehaviour
{
    void Awake() => gameObject.SetActive(Debug.isDebugBuild);
}

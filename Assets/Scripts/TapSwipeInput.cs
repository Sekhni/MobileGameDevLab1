using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TapSwipeInput : MonoBehaviour
{
    public float swipeDp = 50f;     // distance in dp, not pixels
    public float tapMax = 0.3f;     // seconds

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
#if UNITY_EDITOR
        TouchSimulation.Enable();   // lets mouse drags in the Game view act as touches
#endif
    }

    void OnDisable()
    {
#if UNITY_EDITOR
        TouchSimulation.Disable();
#endif
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        if (LifecycleGuard.IsPaused) return;   // ignore swipes and taps while paused

        foreach (var t in Touch.activeTouches)
        {
            if (t.phase != TouchPhase.Ended) continue;
            float px = swipeDp * Mathf.Max(Screen.dpi, 160f) / 160f;
            Vector2 d = t.screenPosition - t.startScreenPosition;
            if (d.magnitude >= px)
            {
                Vector2 dir = d.normalized;
                if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                {
                    if (dir.x > 0) LaneChange.Instance?.MoveRight();
                    else LaneChange.Instance?.MoveLeft();
                }
            }
            else if (t.time - t.startTime < tapMax)
            {
                // Tap could trigger a jump/action later if needed
            }
        }
    }
}

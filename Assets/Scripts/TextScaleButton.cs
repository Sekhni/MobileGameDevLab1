using UnityEngine;

public class TextScaleButton : MonoBehaviour
{
    public void SetFactor(float f)
    {
        Debug.Log("SetFactor " + f + " listeners ok");
        TextScale.Factor = f;
    }
}
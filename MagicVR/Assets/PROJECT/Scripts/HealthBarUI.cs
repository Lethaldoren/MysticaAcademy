using UnityEngine;
using static UnityEngine.RectTransform;

public class HealthBarUI : MonoBehaviour
{
    public RectTransform control;
    public RectTransform image;
    public float minWidth;
    public float maxWidth;

    public void UpdateWidth()
    {
        image.SetSizeWithCurrentAnchors(Axis.Horizontal, Mathf.Clamp(control.rect.width, minWidth, maxWidth));
    }
}

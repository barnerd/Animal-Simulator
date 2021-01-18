using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Image UIPanelBackground;
    public FloatReference UIBackgroundTransparencyValue;

    // called via GameEvent onUIBackgroundTransparencyChange
    public void UpdateBackgroundTransparency()
    {
        var tempColor = UIPanelBackground.color;
        tempColor.a = UIBackgroundTransparencyValue.Value;
        UIPanelBackground.color = tempColor;
    }
}

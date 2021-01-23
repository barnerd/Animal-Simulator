using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    [Header("UI Background Transparency")]
    public FloatReference UIBackgroundTransparencyValue;
    public Text UIBackgroundTransparencyText;
    public Slider UIBackgroundTransparencySlider;
    public GameEvent onUIBackgroundTransparencyChange;

    public void OnUIBackgroundTransparencyChange()
    {
        UIBackgroundTransparencyValue.Value = UIBackgroundTransparencySlider.value;
        onUIBackgroundTransparencyChange.Raise();
    }

    public void UpdateTransparencyText()
    {
        UIBackgroundTransparencyText.text = (UIBackgroundTransparencyValue.Value * 100).ToString("n0") + "%";
    }
}

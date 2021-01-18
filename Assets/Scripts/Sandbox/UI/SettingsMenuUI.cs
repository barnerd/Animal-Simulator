using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    [Header("UI Background Transparency")]
    public FloatReference UIBackgroundTransparencyValue;
    public Text UIBackgroundTransparencyText;
    public Slider UIBackgroundTransparencySlider;
    public GameEvent onUIBackgroundTransparencyChange;

    public void SetTabPanelActive(GameObject activePanel)
    {
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);

        activePanel.SetActive(true);
    }

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

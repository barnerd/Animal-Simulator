using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    [Header("UI Background Transparency")]
    public FloatReference UIBackgroundTransparencyValue;
    public Text UIBackgroundTransparencyText;
    public Slider UIBackgroundTransparencySlider;
    public GameEvent onUIBackgroundTransparencyChange;

    [Header("ToolTip Display Delay")]
    public FloatReference toolTipDisplayDelay;

    [Header("Equipment Layout")]
    public GameObject columnLayout;
    public GameObject bodyLayout;

    [Header("Show Disabled Equipment Slots")]
    public BoolReference showDisabledEquipmentSlots;
    public EquipmentUI equipmentUI;

    public void OnUIBackgroundTransparencyChange()
    {
        UIBackgroundTransparencyValue.Value = UIBackgroundTransparencySlider.value;
        onUIBackgroundTransparencyChange.Raise();
    }

    public void UpdateTransparencyText()
    {
        UIBackgroundTransparencyText.text = (UIBackgroundTransparencyValue.Value * 100).ToString("n0") + "%";
    }

    public void OnToolTipDisplayDelayValueChange(string _value)
    {
        float stringValue;
        if (float.TryParse(_value, out stringValue))
        {
            toolTipDisplayDelay.Value = stringValue;
        }
    }

    public void OnEquipmentLayoutDropdownChange(int _value)
    {
        switch (_value)
        {
            case 0:
                columnLayout.SetActive(true);
                bodyLayout.SetActive(false);
                break;
            case 1:
                columnLayout.SetActive(false);
                bodyLayout.SetActive(true);
                break;
        }
    }

    public void ShowDisabledSlots(bool _value)
    {
        showDisabledEquipmentSlots.Value = _value;
        equipmentUI.UpdateUI(equipmentUI.equipment);
    }
}

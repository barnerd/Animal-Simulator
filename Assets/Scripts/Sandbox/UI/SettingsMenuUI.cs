using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    public Text UIBackgroundTransparencyText;

    public void SetTabPanelActive(GameObject activePanel)
    {
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);

        activePanel.SetActive(true);
    }
}

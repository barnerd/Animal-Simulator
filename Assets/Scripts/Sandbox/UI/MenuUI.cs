using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public GameObject caret;

    public GameObject inventoryButton;
    public GameObject settingsButton;

    public GameObject inventoryUI;
    public GameObject settingsUI;

    public void ToggleMenu()
    {
        caret.transform.Rotate(new Vector3(180, 0, 0));

        inventoryButton.SetActive(!inventoryButton.activeSelf);
        settingsButton.SetActive(!settingsButton.activeSelf);
    }

    public void ToggleInventory()
    {
        settingsUI.SetActive(false);
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    public void ToggleSettings()
    {
        inventoryUI.SetActive(false);
        settingsUI.SetActive(!settingsUI.activeSelf);
    }
}

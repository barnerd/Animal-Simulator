using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public List<GameObject> tabPanels;

    public TabButton activeTab;

    public void Subscribe(TabButton _button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(_button);
    }

    public void OnTabSelected(TabButton _button)
    {
        int index;

        if(activeTab != _button)
        {
            // hide old tab
            if (activeTab != null)
            {
                activeTab.GetComponent<Button>().interactable = true;
                index = activeTab.transform.GetSiblingIndex();
                tabPanels[index].SetActive(false);
            }

            activeTab = _button;

            // show new tab
            activeTab.GetComponent<Button>().interactable = false;
            index = _button.transform.GetSiblingIndex();
            tabPanels[index].SetActive(true);
        }
    }
}

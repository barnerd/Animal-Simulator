using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                index = activeTab.transform.GetSiblingIndex();
                tabPanels[index].SetActive(false);
            }

            activeTab = _button;

            // show new tab
            index = _button.transform.GetSiblingIndex();
            tabPanels[index].SetActive(true);
        }
    }
}

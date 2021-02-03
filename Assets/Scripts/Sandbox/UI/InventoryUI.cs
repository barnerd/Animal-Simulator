﻿using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventorySlotsParentUI;
    InventorySlotUI[] slots;

    public InventoryManager inventory;

    // Start is called before the first frame update
    void Start()
    {
        slots = inventorySlotsParentUI.GetComponentsInChildren<InventorySlotUI>(true);
    }

    public void UpdateUI(MonoBehaviour _inventory)
    {
        if ((InventoryManager)_inventory == inventory)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < inventory.items.Count)
                {
                    slots[i].AddItem(inventory.items[i]);
                }
                else
                {
                    slots[i].Clear();
                }
            }
        }
    }

    public void InteractItem(ItemData _item)
    {
        if (inventory != null)
        {
            inventory.Interact(_item);
        }
    }

    public void DropItem(ItemData _item)
    {
        if (inventory != null)
        {
            inventory.Drop(_item);
        }
    }
}

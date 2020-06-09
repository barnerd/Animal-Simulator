using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUI;
    public GameObject inventorySlotsParentUI;
    InventorySlotUI[] slots;

    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        slots = inventorySlotsParentUI.GetComponentsInChildren<InventorySlotUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    public void UpdateUI(MonoBehaviour _inventory)
    {
        inventory = (Inventory)_inventory;
        Debug.Log("Updating UI from inventory: " + inventory);
        for (int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    public void InteractItem(Item _item)
    {
        if (inventory != null)
        {
            inventory.Interact(_item);
        }
    }

    public void RemoveItem(Item _item)
    {
        if (inventory != null)
        {
            inventory.Remove(_item);
        }
    }
}

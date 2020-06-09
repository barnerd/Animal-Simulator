using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public GameEvent onEquipmentChange;

    public EquipmentSlot[] slots;
    Equipment[] equipment;

    // Start is called before the first frame update
    void Start()
    {
        equipment = new Equipment[slots.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.U))
            UnequipAll();
    }

    public bool Equip(Equipment _item, EquipmentSlot _slot = null)
    {
        EquipmentSlot slot = _slot;

        if (slot == null)
            slot = _item.slot;

        // check if item can be in that slot
        if (_item.slot != slot)
            return false;

        // find slot index, if slot isn't found, return and don't equip item
        int? index = GetSlotIndex(slot);
        if (index == null)
            return false;

        // check if item is already in slot, remove it
        bool success = true;
        if (equipment[index ?? -1] != null)
            success = Unequip(slots[index ?? -1]);

        // add it
        if (success)
        {
            equipment[index ?? -1] = _item;
            //TODO?: Change this call to include which items are changing, or which slots are changing
            onEquipmentChange.Raise(this);
        }

        return success;
    }

    public bool Unequip(EquipmentSlot _slot)
    {
        bool success = true;

        int? index = GetSlotIndex(_slot);
        if (index == null)
            return false;

        Equipment item = equipment[index ?? -1];
        if (item == null)
            return true;

        // TODO:check if _item can be unequiped, i.e. not cursed
        bool unequipable = true;

        success &= unequipable;

        if (success)
        {
            success &= GetComponent<InventoryManager>().Add(item);

            if (success)
            {
                equipment[index ?? -1] = null;
                //TODO?: Change this call to include which items are changing, or which slots are changing
                onEquipmentChange.Raise(this);
            }
        }

        return success;
    }

    public bool UnequipAll()
    {
        bool success = true;

        for (int i = 0; i < slots.Length; i++)
        {
            success &= Unequip(slots[i]);
        }

        return success;
    }

    private int? GetSlotIndex(EquipmentSlot _slot)
    {
        int? index = null;
        for (int i = 0; i < slots.Length; i++)
        {
            index = i;
            if (slots[i] == _slot)
                break;
        }

        if (index == slots.Length)
            index = null;

        return index;
    }
}

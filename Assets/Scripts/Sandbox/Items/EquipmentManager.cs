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
            onEquipmentChange.Raise(this);
            ModifyAttributes(_item, true);
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

        // TODO:check if _item can be unequiped, i.e. not cursed, or default like claws
        bool unequipable = true;

        success &= unequipable;

        if (success)
        {
            success &= GetComponent<InventoryManager>().Add(item);

            if (success)
            {
                equipment[index ?? -1] = null;
                onEquipmentChange.Raise(this);
                ModifyAttributes(item, false);
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

    // TODO: Figure out where this should go. Having it in the equipment manager seems weird, but it's weird all the data is.
    // I wonder if a dictionary, versus a list or array would make this better
    private void ModifyAttributes(Equipment _item, bool add = true)
    {
        CreatureAttributes c = GetComponent<CreatureAttributes>();
        for (int i = 0; i < _item.attributeModifiers.Length; i++)
        {
            for (int j = 0; j < c.attributes.Length; j++)
            {
                if(c.attributes[j].type == _item.attributeModifiers[i].attributeType)
                {
                    if (add)
                    {
                        c.attributes[j].AddModifier(_item.attributeModifiers[i]);
                    }
                    else
                    {
                        c.attributes[j].RemoveModifier(_item.attributeModifiers[i]);
                    }
                }
            }
        }

        for (int i = 0; i < _item.armorModifiers.Length; i++)
        {
            for (int j = 0; j < c.armors.Length; j++)
            {
                if (c.armors[j].damageType == _item.armorModifiers[i].damageType)
                {
                    if (add)
                    {
                        c.armors[j].AddModifier(_item.armorModifiers[i]);
                    }
                    else
                    {
                        c.armors[j].RemoveModifier(_item.armorModifiers[i]);
                    }
                }
            }
        }

        for (int i = 0; i < _item.damageModifiers.Length; i++)
        {
            for (int j = 0; j < c.damages.Length; j++)
            {
                if (c.damages[j].damageType == _item.damageModifiers[i].damageType)
                {
                    if (add)
                    {
                        c.damages[j].AddModifier(_item.damageModifiers[i]);
                    }
                    else
                    {
                        c.damages[j].RemoveModifier(_item.damageModifiers[i]);
                    }
                }
            }
        }
    }
}

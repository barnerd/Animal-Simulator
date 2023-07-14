using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public GameEvent onEquipmentChange;

    public EquipmentSlot[] slots;
    EquipmentData[] equipment;

    public EquipmentData[] baseClothing;

    // Start is called before the first frame update
    void Start()
    {
        equipment = new EquipmentData[slots.Length];

        foreach (var item in baseClothing)
        {
            Equip(item);
        }
    }

    public EquipmentData GetItemData(EquipmentSlot _slot)
    {
        int? i = GetSlotIndex(_slot);

        return (i == null) ? null : equipment[i ?? -1];
    }

    public bool IsValidSlot(EquipmentSlot _slot)
    {
        int? i = GetSlotIndex(_slot);

        return i != null;
    }

    void EquipDefault(EquipmentSlot _slot)
    {
        foreach (var item in baseClothing)
        {
            if (item.slot == _slot)
                Equip(item);
        }
    }

    public bool Equip(EquipmentData _item, EquipmentSlot _slot = null)
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

        // check if an item is already in slot, remove it
        bool success = true;
        if (equipment[index ?? -1] != null)
            success = Unequip(slots[index ?? -1]);

        // everything is good, slot is empty so add it
        if (success)
        {
            equipment[index ?? -1] = _item;
            SetBlendShapes(equipment[index ?? -1], 1);
            UpdateEquipmentGraphics(equipment[index ?? -1]);
            ModifyAttributes(_item, true);
            onEquipmentChange.Raise(this);
        }

        return success;
    }

    public bool Unequip(EquipmentSlot _slot)
    {
        bool success = true;

        // check if this equipmentManager has that slot type
        int? index = GetSlotIndex(_slot);
        if (index == null)
            return false;

        EquipmentData item = equipment[index ?? -1];

        // check if slot is already empty
        if (item == null)
            return true;

        // TODO: check if _item can be unequiped, i.e. not cursed, or default like claws
        bool unequipable = true;

        success &= unequipable;

        // everything is good, slot is ready to be removed
        if (success)
        {
            if (!item.baseClothing)
                success &= GetComponent<InventoryManager>().Add(item);

            if (success)
            {
                // remove old item
                EquipmentData oldItem = equipment[index ?? -1];
                equipment[index ?? -1] = null;
                ModifyAttributes(item, false);
                onEquipmentChange.Raise(this);

                // clear graphics for old item
                SetBlendShapes(oldItem, 0);
                UpdateEquipmentGraphics(oldItem, false);

                // if not remove baseClothing, add baseClothing
                if (!oldItem.baseClothing)
                    EquipDefault(_slot);
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
        bool found = false;

        for (int i = 0; i < slots.Length; i++)
        {
            index = i;
            if (slots[i] == _slot)
            {
                found = true;
                break;
            }
        }

        if (!found)
            index = null;

        return index;
    }

    void UpdateEquipmentGraphics(EquipmentData _item, bool _add = true)
    {
        if (_add)
            GetComponentInChildren<CreatureGFX>().AddEquipmentGraphics(_item);
        else
            GetComponentInChildren<CreatureGFX>().RemoveEquipmentGraphics(_item);
    }

    void SetBlendShapes(EquipmentData _item, float _weight)
    {
        GetComponentInChildren<CreatureGFX>().SetBlendShapes(_item.blendShapeValues, _weight);
    }

    private void ModifyAttributes(EquipmentData _item, bool add = true)
    {
        CreatureAttributes _creatureAttributes = GetComponent<CreatureAttributes>();

        foreach (var attributeModifier in _item.attributeModifiers)
        {
            _creatureAttributes.AddAttributeModifier(attributeModifier, attributeModifier.attributeType, add);
        }

        foreach (var armorModifier in _item.armorModifiers)
        {
            _creatureAttributes.AddArmorModifier(armorModifier, armorModifier.damageType, add);
        }

        foreach (var damageModifier in _item.damageModifiers)
        {
            _creatureAttributes.AddDamageModifier(damageModifier, damageModifier.damageType, add);
        }
    }
}

using UnityEngine;
using TMPro;

public class EquipmentUI : MonoBehaviour
{
    public GameObject equipmentSlotsParentUI;
    EquipmentSlotUI[] slots;
    public TMP_Text totalDamageText;
    public TMP_Text totalArmorText;

    public EquipmentManager equipment;

    // Start is called before the first frame update
    void Start()
    {
        slots = equipmentSlotsParentUI.GetComponentsInChildren<EquipmentSlotUI>(true);
    }

    /// <summary>
    /// Update the Equipment screen, usually when OnEquipmentChange is called
    /// </summary>
    /// <param name="_equipment">The Equipment Manager that's changed</param>
    public void UpdateUI(MonoBehaviour _equipment)
    {
        if ((EquipmentManager)_equipment == equipment)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (!equipment.IsValidSlot(slots[i].slotType))
                {
                    slots[i].disabled = true;
                }

                EquipmentData item = equipment.GetItemData(slots[i].slotType);
                if (item != null)
                {
                    slots[i].DisplayItem(item);
                }
                else
                {
                    slots[i].Clear();
                }
            }

            // Update Total Damage/Armor text
            CreatureAttributes c = equipment.GetComponent<CreatureAttributes>();
            totalDamageText.text = c.GetMinTotalDamage() + "-" + c.GetMaxTotalDamage();
            totalArmorText.text = c.GetTotalArmor().ToString();
        }
    }

    public void UnequipItem(EquipmentSlot _slot)
    {
        if (equipment != null)
        {
            equipment.Unequip(_slot);
        }
    }
}

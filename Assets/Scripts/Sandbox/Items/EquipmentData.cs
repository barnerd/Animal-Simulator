using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Item/Equipment Data")]
public class EquipmentData : ItemData
{
    [Header("Equipment")]
    // TODO: extend this to multiple slots
    // i.e. mainHand & offHand is twoHanded
    // or torso & shoulders is connected armor
    // or torso & back is caped armor
    public EquipmentSlot slot;
    public bool baseClothing = false;

    public SkinnedMeshRenderer skinnedMesh;
    // TODO: Turn this into a dictionary and have it show up in the editor
    //public Dictionary<EquipmentMeshBlendShape, float> blendShapeValues;
    public EquipmentMeshBlendShape[] blendShapeValues;

    [Header("Modifiers")]
    public AttributeModifier[] attributeModifiers;
    public ArmorModifier[] armorModifiers;
    public DamageModifier[] damageModifiers;

    public override void Interact(MonoBehaviour _actor)
    {
        base.Interact(_actor);

        if(_actor.GetComponent<EquipmentManager>().Equip(this))
        {
            _actor.GetComponent<InventoryManager>().Remove(this);
        }
    }
}

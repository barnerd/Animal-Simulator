using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Item/Equipment")]
public class Equipment : Item
{
    [Header("Equipment")]
    public EquipmentSlot slot;
    // TODO: Create Editor for AttributeModifier
    public AttributeModifier[] statModifiers;

    public override void Interact(MonoBehaviour _actor)
    {
        base.Interact(_actor);

        if(_actor.GetComponent<EquipmentManager>().Equip(this))
        {
            _actor.GetComponent<InventoryManager>().Remove(this);
        }
    }
}

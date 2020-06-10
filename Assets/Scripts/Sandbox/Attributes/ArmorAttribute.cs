using UnityEngine;

[System.Serializable]
public class ArmorAttribute : Attribute
{
    public DamageType damageType;

    public ArmorAttribute(float _base) : base(_base)
    {
    }

    public void AddArmorModifier(ArmorModifier _modifier)
    {
        if (_modifier.damageType == this.damageType)
        {
            AddModifier(_modifier);
        }
    }
}

using UnityEngine;

[System.Serializable]
public class ArmorAttribute : Attribute
{
    public DamageType damageType;

    public ArmorAttribute(float _base) : base(_base)
    {
    }
}

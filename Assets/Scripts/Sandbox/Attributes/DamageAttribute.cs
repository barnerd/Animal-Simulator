using UnityEngine;

[System.Serializable]
public class DamageAttribute : Attribute
{
    public DamageType damageType;
    public int dice;
    public float sides;

    public DamageAttribute(float _base, int _dice = 0, float _sides = 0) : base(_base)
    {
        dice = _dice;
        sides = _sides;
    }

    public float GetDamage()
    {
        currentValue = BaseValue;
        for (int i = 0; i < dice; i++)
        {
            currentValue += Random.Range(1f, sides);
        }
        return currentValue;
    }
}

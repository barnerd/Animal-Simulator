using UnityEngine;

[System.Serializable]
public class DamageAttribute : Attribute
{
    public DamageType damageType;
    public float min;
    public float max;
    // TODO: represent as dice damage adb+c, where a is number of dice, b is sides on each dice and c is a base damage

    public DamageAttribute(float _base, float _max = 0, float _min = 0) : base(_base)
    {
        max = _max;
        min = _min;
    }

    public float GetDamage()
    {
        currentValue = Random.Range(min, max) + BaseValue;
        return currentValue;
    }
}

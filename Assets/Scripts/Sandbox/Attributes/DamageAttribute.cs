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

    public void AddDamageModifier(DamageModifier _modifier)
    {
        if (_modifier.damageType == this.damageType)
        {
            AddModifier(_modifier);

        }
    }

    public float GetDamage()
    {
        currentValue = BaseValue;
        for (int i = 0; i < dice; i++)
        {
            currentValue += Random.Range(1f, sides);
        }

        // add modifiers
        foreach (var modifer in modifiers)
        {
            //TODO: check that casting is possible
            DamageModifier dm = (DamageModifier)modifer;
            currentValue += dm.modifier;
            for (int i = 0; i < dm.dice; i++)
            {
                currentValue += Random.Range(1f, dm.sides);
            }
        }

        return currentValue;
    }

    /// <summary>
    /// Gets the minimum potential damage, including all types
    /// </summary>
    /// <returns>minimum potential damage</returns>
    public float GetMinDamage()
    {
        float damage = BaseValue;
        damage += dice;

        // add modifiers
        foreach (var modifer in modifiers)
        {
            DamageModifier dm = (DamageModifier)modifer;
            damage += dm.modifier;
            damage += dm.dice;
        }

        return damage;
    }

    /// <summary>
    /// Gets the maximum potential damage, including all types
    /// </summary>
    /// <returns>maximum potential damage</returns>
    public float GetMaxDamage()
    {
        float damage = BaseValue;
        damage += dice * sides;

        // add modifiers
        foreach (var modifer in modifiers)
        {
            //TODO: check that casting is possible
            DamageModifier dm = (DamageModifier)modifer;
            damage += dm.modifier;
            damage += dm.dice * dm.sides;
        }

        return damage;
    }
}

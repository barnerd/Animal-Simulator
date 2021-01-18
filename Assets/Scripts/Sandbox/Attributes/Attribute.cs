using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attribute
{
    public AttributeType type;
    public float BaseValue { get; }
    public float currentValue { get; protected set; }

    protected List<AttributeModifier> modifiers;

    public Attribute(float _base)
    {
        BaseValue = _base;
        currentValue = BaseValue;

        modifiers = new List<AttributeModifier>();
    }

    public void AddModifier(AttributeModifier _modifier)
    {
        if (_modifier != null && _modifier.attributeType == type)
        {
            modifiers.Add(_modifier);
            currentValue += _modifier.modifier;
        }
    }

    public void RemoveModifier(AttributeModifier _modifier)
    {
        if (_modifier != null)
        {
            modifiers.Remove(_modifier);
            currentValue -= _modifier.modifier;
        }
    }
}

/*
 * make these attribute types, like you did equipment slots. then create an attribute, which is a type and a value. then add an attribute list to each creature
 * 
 * strength - attribute types
 * intelligence - attribute
 * dexterity - attribute
 * 
 * armor - damage type, value
 * elemental resistance = percentages of the damage. - TODO
 * damage - base, min, max, can be represented as dice 2d5 = [2,10] 3d7+2 = [5,23], has damage type. 
 * damage type: slash, piece, blunt, fire
 *  
 * health - max, current, current%, effect at 0, i.e. has event(s) to call - TODO - metered
 * hunger - extended attribute
 * thirst
 * stamina
 * energy
 * 
 * fishing - skill - TODO
 * blacksmith - skill
 * farming - skill
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAttributes : MonoBehaviour
{
    public Attribute[] attributes;
    public ArmorAttribute[] armors;
    public DamageAttribute[] damages;
    public MeteredAttribute[] meters;

    // Update is called once per frame
    void Update()
    {
        // for testing
        if (Input.GetKeyUp(KeyCode.T))
        {
            TakeDamage(damages[2].damageType, damages[2].GetDamage());
        }
    }

    public float? GetAttributeCurrentValue(AttributeType _type)
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            if (attributes[i].type == _type)
                return attributes[i].currentValue;
        }

        return null;
    }

    public void ChangeMeter(AttributeType _type, float _delta)
    {
        for (int i = 0; i < meters.Length; i++)
        {
            if (meters[i].type == _type)
            {
                meters[i].ChangeMeter(_delta, GetComponent<Creature>());
                Debug.Log(name + " takes " + _delta + " damage against " + _type + " type.");
            }
        }
    }

    public void TakeDamage(DamageType _type, float _damage)
    {
        float _delta = _damage;

        for (int i = 0; i < armors.Length; i++)
        {
            if (armors[i].damageType == _type)
            {
                _delta -= armors[i].currentValue;
            }
        }

        if (_delta < 0)
            _delta = 0;

        // TODO: don't hardcode index 0 here
        ChangeMeter(meters[0].type, -_delta);
        Debug.Log(name + " takes " + _delta + " damage of type " + _type + ".");
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAttributes : MonoBehaviour
{
    public Attribute[] attributes;
    public ArmorAttribute[] armors;
    public DamageAttribute[] damages;
    public MeteredAttribute[] meters;

    //TODO: have a better way of setting up all attributes
    void Awake()
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            AttributeType currentType = attributes[i].type;
            attributes[i] = new Attribute(8);
            attributes[i].type = currentType;
        }

        for (int i = 0; i < armors.Length; i++)
        {
            AttributeType currentType = armors[i].type;
            DamageType currentDamageType = armors[i].damageType;
            armors[i] = new ArmorAttribute(0);
            armors[i].type = currentType;
            armors[i].damageType = currentDamageType;
        }

        for (int i = 0; i < damages.Length; i++)
        {
            AttributeType currentType = damages[i].type;
            DamageType currentDamageType = damages[i].damageType;
            damages[i] = new DamageAttribute(0);
            damages[i].type = currentType;
            damages[i].damageType = currentDamageType;
        }

        for (int i = 0; i < meters.Length; i++)
        {
            AttributeType currentType = meters[i].type;
            meters[i] = new MeteredAttribute(100);
            meters[i].type = currentType;
        }
    }

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

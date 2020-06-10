using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public InputController currentController;

    // TODO: Move speed to stats
    //public float speed = 6f;

    public Attribute[] attributes;
    public ArmorAttribute[] armors;
    public DamageAttribute[] damages;
    public MeteredAttribute[] meters;

    //public Interactable focus;

    // Start is called before the first frame update
    void Start()
    {
        currentController.Initialize(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        currentController.ProcessInput(this.gameObject);

        // for testings
        if (Input.GetKeyUp(KeyCode.T))
        {
            TakeDamage(damages[0].damageType, 5);
        }
    }

    // FixedUpdate is used with physics
    void FixedUpdate()
    {
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

    public bool Interact(Interactable _focus)
    {
        return _focus.Interact(this.gameObject);
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

    public void ChangeMeter(AttributeType _type, float _delta)
    {
        for (int i = 0; i < meters.Length; i++)
        {
            if (meters[i].type == _type)
            {
                meters[i].ChangeMeter(_delta, this);
                Debug.Log(name + " takes " + _delta + " damage against " + _type + " type.");
            }
        }
    }

    public virtual void Die()
    {
        Debug.Log(name + " dies.");
    }
}

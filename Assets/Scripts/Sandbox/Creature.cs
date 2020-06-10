using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public InputController currentController;

    // TODO: Move speed to stats
    public float speed = 6f;

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
    }

    // FixedUpdate is used with physics
    void FixedUpdate()
    {
    }

    public bool Interact(Interactable _focus)
    {
        return _focus.Interact(this.gameObject);
    }
}

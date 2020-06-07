﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public InputController currentController;

    // TODO: Move speed to stats
    public float speed = 6f;

    public Interactable focus;

    // Start is called before the first frame update
    void Start()
    {
        currentController.Initialize(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (focus != null)
        {
            GetComponent<CreatureMotor>().FaceTarget(focus.transform);
        }
    }

    // FixedUpdate is used with physics
    void FixedUpdate()
    {
        currentController.ProcessInput(this.gameObject);
    }

    public void SetFocus(Interactable _focus)
    {
        focus = _focus;
    }

    public void RemoveFocus()
    {
        focus = null;
    }
}

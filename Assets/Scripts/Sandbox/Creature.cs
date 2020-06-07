using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public InputController currentController;

    // TODO: Move speed to stats
    public float speed = 6f;

    // Start is called before the first frame update
    void Start()
    {
        currentController.Initialize(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // FixedUpdate is used with physics
    void FixedUpdate()
    {
        currentController.ProcessInput(this.gameObject);
    }
}

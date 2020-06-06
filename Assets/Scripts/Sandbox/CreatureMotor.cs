using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMotor : MonoBehaviour
{
    public Rigidbody rb;
    public float thrust = 0f;

    public float speed = 1000f;
    public float maxSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.right * thrust);
    }

    public void MoveForward()
    {

    }

    public void MoveSideways(float input)
    {
        thrust = input * speed;
    }
}

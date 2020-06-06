using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMotor : MonoBehaviour
{
    public CharacterController controller;

    //public Rigidbody rb;
    public float gravity = -9.81f;
    public float jumpHeight = .5f;

    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        if (controller == null) controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = gravity;
        }
        velocity.y += .5f * gravity * Time.deltaTime * Time.deltaTime;
        controller.Move(velocity);
    }

    void FixedUpdate()
    {
    }

    public void Move(Vector3 _direction)
    {
        controller.Move(_direction);
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity) * Time.deltaTime;
        }
    }
}

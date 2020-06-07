using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMotor : MonoBehaviour
{
    public CharacterController controller;

    public float gravity = -9.81f;
    public float jumpHeight = .5f;

    Vector3 velocity;
    public Vector3 direction;
    public Vector3 targetPosition;

    public float turnSmoothTime = 0.6f;
    private float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        if (controller == null)
        {
            if (gameObject.GetComponent<CharacterController>() == null)
            {
                controller = gameObject.AddComponent<CharacterController>();
            }
            else
            {
                controller = gameObject.GetComponent<CharacterController>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = gravity;
        }
        velocity.y += .5f * gravity * Time.deltaTime * Time.deltaTime;

        controller.Move(velocity + direction * this.gameObject.GetComponent<Creature>().speed * Time.deltaTime);
    }

    void FixedUpdate()
    {
    }

    public void Move(Vector3 _direction)
    {
        // if input, then move in direction
        if (_direction.magnitude >= .1f)
        {
            float angle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
            float targetAngle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, angle, ref turnSmoothVelocity, turnSmoothTime);
            this.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
        this.direction = _direction;
    }

    public void MoveToPoint(Vector3 _point)
    {
        this.targetPosition = _point;
    }

    public void FaceTarget(Transform _target)
    {
        Vector3 lookDirection = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0f, lookDirection.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 2f * Time.deltaTime);
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity) * Time.deltaTime;
        }
    }
}

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

    public AttributeType speed;

    Creature creature;

    // targets
    Transform lookAtTarget;
    Transform faceTarget;
    Transform followTarget;

    //
    Vector3 faceDirection;

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

        creature = gameObject.GetComponent<Creature>();

        // initialize variables
        faceDirection = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = gravity;
        }
        velocity.y += .5f * gravity * Time.deltaTime * Time.deltaTime;

        float? creatureSpeed = creature.GetAttributeCurrentValue(speed);

        controller.Move(velocity + direction * (creatureSpeed ?? 0) * Time.deltaTime);

        // if face direction and transform.forward are too far apart
        if (Vector3.Angle(faceDirection, transform.forward) > 5f)
            RotateCreature();
    }

    void FixedUpdate()
    {
    }

    private void RotateCreature()
    {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(faceDirection.x, 0f, faceDirection.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 2f * Time.deltaTime);
    }

    #region Move functions
    public void MoveForward()
    {
        MoveDirection(transform.forward);
    }

    public void MoveDirection(Vector3 _direction)
    {
        this.direction = _direction;
    }

    public void MoveToPosition(Vector3 _position)
    {
        MoveDirection(_position - transform.position);
    }
    #endregion

    #region Look functions
    public void LookAtTransform(Transform _target)
    {
        lookAtTarget = _target;
        LookAtPosition(_target.position);
    }

    public void LookAtPosition(Vector3 _position)
    {
        LookAtDirection(_position - transform.position);
    }

    public void LookAtDirection(Vector3 _direction)
    {
        // TODO: implement look at, for now face direction
        FaceDirection(_direction);
    }

    public void LookForward()
    {
        LookAtDirection(transform.forward);
    }
    #endregion

    #region Face functions
    public void FaceTransform(Transform _target)
    {
        faceTarget = _target;
        FacePosition(_target.position);
    }

    public void FacePosition(Vector3 _position)
    {
        FaceDirection(_position - transform.position);
    }

    public void FaceDirection(Vector3 _direction)
    {
        faceDirection = _direction.normalized;
    }

    public void FaceForward()
    {
        FaceDirection(transform.forward);
    }
    #endregion

    public void Follow(Transform _target, float _radius)
    {
        followTarget = _target;
        FaceTransform(_target);
        MoveToPosition(_target.position);
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity) * Time.deltaTime;
        }
    }

    public void Crouch()
    {
        //TODO: implement crouching
    }

    public void Strafe()
    {
        //TODO: implement strafe
    }

    public void Swim()
    {
        //TODO: implement swimming
    }
}

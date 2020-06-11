using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMotor : MonoBehaviour
{
    public CharacterController controller;

    public float gravity = -9.81f;
    public float jumpHeight = .5f;
    public float rotationSpeed = .5f;
    public float minFaceDirectionDelta = 1f;
    public float minLookDirectionDelta = 1f;
    public float closingRadius = 5f;

    Vector3 velocity;

    public AttributeType speed;

    Creature creature;
    CreatureAttributes creatureAttributes;

    // targets
    Vector3? moveToPosition;
    Transform moveTarget;
    Transform faceTarget;
    Transform lookAtTarget;
    Transform followTarget;

    // radii
    float moveRadius;
    float followRadius;

    // directions
    Vector3 moveDirection;
    Vector3 faceDirection;
    Vector3 lookDirection;

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
        creatureAttributes = creature.GetComponent<CreatureAttributes>();

        // initialize variables
        moveDirection = Vector3.zero;
        faceDirection = transform.forward;
        lookDirection = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToPosition != null)
        {
            MoveToPosition(moveToPosition ?? transform.position);
        }

        if (moveTarget != null)
        {
            if(Vector3.Distance(transform.position, moveTarget.position) > moveRadius)
            {
                MoveToTransform(moveTarget, moveRadius);
            }
            else
            {
                moveTarget = null;
                moveToPosition = null;
            }
        }

        if (faceTarget != null)
        {
            FaceTransform(faceTarget);
        }

        if (lookAtTarget != null)
        {
            LookAtTransform(lookAtTarget);
        }

        if (followTarget != null)
        {
            Follow(followTarget, followRadius);
        }

        // TODO: implement look at
        /*
         * if (Vector3.Angle(lookDirection, VectorForLooking) > minLookDirectionDelta)
         *      MakeCreaturesHeadMove();
         * */

        // if face direction and transform.forward are too far apart
        if (Vector3.Angle(faceDirection, transform.forward) > minFaceDirectionDelta)
            RotateCreature();

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = gravity;
        }
        velocity.y += .5f * gravity * Time.deltaTime * Time.deltaTime;

        float? creatureSpeed = creatureAttributes.GetAttributeCurrentValue(speed);

        controller.Move(velocity + moveDirection * (creatureSpeed ?? 0) * Time.deltaTime);
    }

    void FixedUpdate()
    {
    }

    private void RotateCreature()
    {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(faceDirection.x, 0f, faceDirection.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    #region Move functions
    public void MoveToTransform(Transform _target, float _radius)
    {
        moveRadius = _radius;
        moveTarget = _target;
        FaceTransform(_target);
        MoveToPosition(_target.position);
    }

    public void MoveToPosition(Vector3 _position)
    {
        moveToPosition = _position;
        MoveDirection(_position - transform.position);
    }

    public void MoveDirection(Vector3 _direction)
    {
        this.moveDirection = _direction.normalized;
        FaceDirection(_direction);
    }

    public void MoveForward()
    {
        this.moveDirection = transform.forward;
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
        lookDirection = _direction.normalized;
    }

    public void LookForward()
    {
        LookAtDirection(transform.forward);
    }
    #endregion

    public void ClearTargets()
    {
        moveTarget = null;
        moveToPosition = null;
        faceTarget = null;
        lookAtTarget = null;
        followTarget = null;
    }

    public void Follow(Transform _target, float _radius)
    {
        followRadius = _radius;
        followTarget = _target;
        MoveToTransform(_target, _radius);
    }

    public void Unfollow()
    {
        followTarget = null;
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity) * Time.deltaTime;
        }
    }

    public void Swing()
    {
        //TODO: implement swing
    }

    public void Throw()
    {
        //TODO: implement throw
    }

    public void Drop()
    {
        //TODO: implement drop
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Creature), typeof(CreatureAttributes))]
public class CreatureMotor : MonoBehaviour
{
    [SerializeField]
    private Rigidbody creatureRigidbody;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Creature creature;
    [SerializeField]
    private CreatureAttributes creatureAttributes;

    [Header("Player")]
    [Tooltip("Walking move speed of player, in m/s")]
    [SerializeField]
    private float walkMoveSpeed = 2.0f;
    [Tooltip("Sprint move speed of player, in m/s")]
    [SerializeField]
    private float sprintMoveSpeed = 5.3f;
    [Tooltip("Time to rotation to face direction")]
    [Range(0.0f, 0.3f)]
    [SerializeField]
    private float rotationSmoothTime = .12f;
    [SerializeField]
    private float speedChangeRate = 10.0f;

    [Space(10)]
    [Tooltip("Height the player jumps to, in m")]
    [SerializeField]
    private float jumpHeight = 1.2f;
    [Tooltip("Extra gravity for the player")]
    [SerializeField]
    private float gravity = -15.0f;
    public LayerMask Ground;
    private bool isGrounded;
    [SerializeField]
    private Transform _groundChecker;

    [Space(10)]
    [Tooltip("Time before next jump")]
    [SerializeField]
    private float jumpTimeout = 0.5f;
    private float jumpTimeoutDelta;
    [Tooltip("Time in air before falling")]
    [SerializeField]
    private float fallTimeout = 0.15f;
    private float fallTimeoutDelta;

    [Tooltip("Speed difference to begin smoothing")]
    [SerializeField]
    private float speedSmoothThreshold = 0.1f;
    public float rotationSpeed = .5f;
    public float minFaceDirectionDelta = 1f;
    public float minLookDirectionDelta = 1f;
    public float closingRadius = 5f;

    [SerializeField]
    private bool isSprinting = false;
    [SerializeField]
    private bool isJumping;
    [SerializeField]
    private bool readyToJump = false;
    [SerializeField]
    private bool isFalling;

    Vector3 velocity;

    public AttributeType speed;

    private float _speed;
    private float verticalVelocity;
    [SerializeField]
    private float terminalVelocity = 53.0f;
    // targetRotation;
    // rotationVelocity;

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
    private Vector3 inputDirection;
    private Vector3 faceDirection;
    private Vector3 lookDirection;
    private Vector3 targetDirection;

    // Start is called before the first frame update
    void Start()
    {
        if (creature == null) creature = GetComponent<Creature>();
        if (creatureAttributes == null) creatureAttributes = GetComponent<CreatureAttributes>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        // initialize variables
        inputDirection = Vector3.zero;
        faceDirection = transform.forward;
        lookDirection = transform.forward;

        jumpTimeoutDelta = jumpTimeout;
        fallTimeoutDelta = fallTimeout;
    }

    // Update is called once per frame
    void Update()
    {
        JumpAndGravity();
        Move();
    }

    void FixedUpdate()
    {
        creatureRigidbody.MovePosition(creatureRigidbody.position + targetDirection * _speed * Time.fixedDeltaTime);
    }

    private void Move()
    {
        //float? creatureSpeed = creatureAttributes.GetAttributeCurrentValue(speed);

        // use creatureSpeed in figuring out speed
        float targetSpeed = (isSprinting ? sprintMoveSpeed : walkMoveSpeed);

        if (inputDirection == Vector3.zero)
        {
            targetSpeed = 0.0f;
        }

        //float currentHorizontalSpeed = new Vector3(creatureRigidbody.velocity.x, 0.0f, creatureRigidbody.velocity.z).magnitude;
        float currentHorizontalSpeed = _speed;

        if (currentHorizontalSpeed < targetSpeed - speedSmoothThreshold || currentHorizontalSpeed > targetSpeed + speedSmoothThreshold)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputDirection.magnitude, Time.deltaTime * speedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        // call Animator to blend velocity
        animator.SetFloat("speedPercent", _speed / walkMoveSpeed);

        float targetAngle = 0.0f;

        if (inputDirection != Vector3.zero)
        {
            targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }

        targetDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;


        //controller.Move(new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime + targetDirection.normalized * _speed * Time.deltaTime);

        creature.FindClosestInteractable();
    }

    private void JumpAndGravity()
    {
        isGrounded = Physics.CheckSphere(_groundChecker.position, 0.2f, Ground, QueryTriggerInteraction.Ignore);

        if (isGrounded)
        {
            isFalling = false;
            isJumping = false;
            fallTimeoutDelta = fallTimeout;

            // call Animator to cancel falling & jumping

            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = 0.0f;

            }

            if (readyToJump)
            {
                //verticalVelocity = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
                creatureRigidbody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * gravity), ForceMode.VelocityChange);

                readyToJump = false;
                isJumping = true;

                // call Animator to change to jumping animation
            }

            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            jumpTimeoutDelta = jumpTimeout;

            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                isFalling = true;
                // call Animator to falling
            }

            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity += gravity * Time.deltaTime;
            }
            else
            {
                // if verticalVelocity is too high, call Animator to death fall and kill character
            }
        }
    }

    #region Move functions
    public void MoveToTransform(Transform _target, float _radius)
    {
        moveRadius = _radius;
        moveTarget = _target;
        FacePosition(_target);
        MoveToPosition(_target.position);
    }

    public void MoveToPosition(Vector3 _position)
    {
        moveToPosition = _position;
        MoveDirection(_position - transform.position);
    }

    public void MoveDirection(Vector3 _direction)
    {
        inputDirection = _direction;
    }

    public void MoveForward()
    {
        MoveDirection(transform.forward);
    }
    #endregion

    #region Face functions
    public void FacePosition(Transform _target)
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
    public void LookAtPosition(Transform _target)
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
        if (isGrounded && jumpTimeoutDelta < 0.0f)
        {
            readyToJump = true;
        }
    }

    public void Sprint()
    {
        isSprinting = !isSprinting;
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

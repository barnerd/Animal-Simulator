using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using BarNerdGames;

[CreateAssetMenu(fileName = "InputController", menuName = "Input Controller/Player Controller")]
public class PlayerController : InputController
{
    private InputControls inputControls;

    public override void Initialize(GameObject obj)
    {
        inputControls = new InputControls();
        inputControls.Enable();
    }

    public override void ProcessInput(GameObject obj)
    {
        CreatureMotor motor = obj.GetComponent<CreatureMotor>();
        Creature creature = obj.GetComponent<Creature>();

        // get move input
        Vector2 moveInput = inputControls.Land.Move.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        if (moveDirection.magnitude != 0f)
        {
            float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        motor.MoveDirection(moveDirection);

        //motor.FaceDirection(lookDirection);

        // jump
        if (inputControls.Land.Jump.triggered)
        {
            motor.Jump();
        }

        // sprint
        if (inputControls.Land.Sprint.triggered)
        {
            motor.Sprint();
        }

        if (inputControls.Land.Attack1.triggered)
        {
            Debug.Log("Attack1 triggered");
        }

        if (inputControls.Land.Attack2.triggered)
        {
            Debug.Log("Attack2 triggered");
        }

        if (inputControls.Land.Attack3.triggered)
        {
            Debug.Log("Attack3 triggered");
        }

        if (inputControls.Land.Crouch.triggered)
        {
            Debug.Log("Crouch triggered");
        }

        if (inputControls.Land.Interact.triggered)
        {
            Debug.Log("Interact triggered");
            creature.Interact();
        }

        // get interact working and remove this raycasting on mouse click
        // use left mouse button to interact
        /*
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // check if we hit interactable
                Interactable interactable = hit.collider.GetComponentInParent<Interactable>();

                // if yes, do something
                if (interactable != null)
                {
                    if (Vector3.Distance(obj.transform.position, interactable.transform.position) < interactable.radius)
                    {
                        obj.GetComponent<Creature>().Interact(interactable);
                    }
                    else
                    {
                        Debug.Log("out of range of interactable");
                        obj.GetComponent<CreatureMotor>().MoveToTransform(interactable.transform, interactable.radius);
                    }
                }
            }
        }*/
    }
}

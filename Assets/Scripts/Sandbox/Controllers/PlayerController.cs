using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "InputController", menuName = "Input Controller/Player Controller")]
public class PlayerController : InputController
{
    public Camera cam;

    public override void Initialize(GameObject obj)
    {
        if (cam != null)
        {
            cam.transform.parent = obj.transform;
        }
    }

    public override void ProcessInput(GameObject obj)
    {
        // get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = obj.transform.forward * vertical;
        Vector3 lookDirection = obj.transform.right * horizontal;

        obj.GetComponent<CreatureMotor>().MoveDirection(direction);
        obj.GetComponent<CreatureMotor>().FaceDirection(lookDirection);

        // jump
        if (Input.GetButtonDown("Jump"))
        {
            obj.GetComponent<CreatureMotor>().Jump();
        }

        // use left mouse button to interact
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
                    if (Vector3.Distance(obj.transform.position, interactable.transform.position) < interactable.radius * .99f)
                    {
                        obj.GetComponent<Creature>().Interact(interactable);
                    }
                    else
                    {
                        Debug.Log("out of range of interactable");
                    }
                }
            }
        }
    }
}
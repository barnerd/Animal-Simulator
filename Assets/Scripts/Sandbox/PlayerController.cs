using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    //public CreatureMotor motor;
    public CharacterController motor;

    // TODO: Move speed to stats
    public float speed = 6f;

    public float turnSmoothTime = 0.3f;
    private float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        if(cam != null)
        {
            cam.transform.parent = this.transform;
        }

        if(motor == null)
        {
            motor = this.GetComponent<CharacterController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // if input, then move in direction
        if (direction.magnitude >= .1f)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float targetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            motor.Move(direction * speed * Time.deltaTime);
        }

        // use left mouse button to interact
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // check if we hit interactable
                // if yes, do something
            }
        }
    }
}

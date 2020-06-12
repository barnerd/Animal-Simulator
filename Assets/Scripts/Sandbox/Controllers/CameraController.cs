using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;

    public float zoomSpeed = 4f;
    public float minZoom = 6f;
    public float maxZoom = 10f;
    private float currentZoom = 9f;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            target = this.transform;
            transform.parent = target.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // get current zoom from scrollwheel
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    void LateUpdate()
    {
        // set position based on target, offset and zoom
        transform.position = target.position - offset * currentZoom;

        // face the same way as the target is
        transform.RotateAround(target.position, Vector3.up, target.eulerAngles.y);

        //look at the target
        transform.LookAt(target.position);

    }
}

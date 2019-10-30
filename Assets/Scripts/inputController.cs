using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputController : MonoBehaviour
{
    private float yaw;
    private float pitch;
    private float roll;
    private float x;
    private float y;
    private float z;
    private Transform newTransform;

    public float positionLerpTime = 0.2f;
    public float rotationLerpTime = 0.01f;
    public float angleToRotate;

    float positionLerpPct;
    float rotationLerpPct;


    // Start is called before the first frame update
    void Start()
    {
        newTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Framerate-independent interpolation
        // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
        positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
        rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);

        MoveTargetPosition();
        UpdatePosition();
    }

    Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        return direction;
    }

    float GetInputRotation()
    {
        float angle = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            angle -= angleToRotate;
        }
        if (Input.GetKey(KeyCode.E))
        {
            angle += angleToRotate;
        }
        return angle;
    }

    void MoveTargetPosition()
    {
        // Translation
        var translation = GetInputTranslationDirection() * Time.deltaTime;
        var rotation = GetInputRotation() * Time.deltaTime;

        // Speed up movement when shift key held
        if (Input.GetKey(KeyCode.LeftShift))
        {
            translation *= 10.0f;
        }

        yaw = newTransform.eulerAngles.y;
        yaw += rotation;
        newTransform.eulerAngles = new Vector3(newTransform.eulerAngles.x, yaw, newTransform.eulerAngles.z);
        Vector3 rotatedTranslation = Quaternion.Euler(newTransform.eulerAngles.x, newTransform.eulerAngles.y, newTransform.eulerAngles.z) * translation;

        newTransform.position += rotatedTranslation;
    }

    private void UpdatePosition()
    {
        pitch = Mathf.Lerp(transform.eulerAngles.x, newTransform.eulerAngles.x, rotationLerpPct);
        yaw = Mathf.Lerp(transform.eulerAngles.y, newTransform.eulerAngles.y, rotationLerpPct);
        roll = Mathf.Lerp(transform.eulerAngles.z, newTransform.eulerAngles.z, rotationLerpPct);

        x = Mathf.Lerp(transform.position.x, newTransform.position.x, positionLerpPct);
        y = Mathf.Lerp(transform.position.y, newTransform.position.y, positionLerpPct);
        z = Mathf.Lerp(transform.position.z, newTransform.position.z, positionLerpPct);

        transform.eulerAngles = new Vector3(pitch, yaw, roll);
        transform.position = new Vector3(x, y, z);
    }
}

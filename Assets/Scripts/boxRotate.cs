using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxRotate : MonoBehaviour
{
    public Vector3 angle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(angle, 45 * Time.deltaTime);

        //transform.RotateAround(Vector3.zero, angle, 45 * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.forward, 45 * Time.deltaTime);
    }
}

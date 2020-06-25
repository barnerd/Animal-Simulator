using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunLight : MonoBehaviour
{
    Light sunLight;
    public Transform target;
    public Gradient lightColor;

    // Start is called before the first frame update
    void Start()
    {
        sunLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.position);

        // find angle of the sunLight
        float dotProduct = Vector3.Dot(Vector3.left, (-transform.position).normalized);
        sunLight.color = lightColor.Evaluate(-dotProduct / 2 + .5f);
    }
}

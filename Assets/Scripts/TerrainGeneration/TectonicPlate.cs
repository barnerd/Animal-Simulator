using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TectonicPlate
{
    public const float maxSpeed = 5f; // this is the sqrt of the max speed
    public const float maxRotation = Mathf.PI / 100; // this angle of rotation
    public int size;
    public Vector2 center;
    public Vector2 direction;
    public float speed;
    public float rotationAngle;
    public bool oceanic; // if false, then continental

    public TectonicPlate(Vector2 _direction, float _rotationAngle, bool _oceanic = true)
    {
        direction = _direction;
        speed = direction.magnitude;
        direction.Normalize();
        rotationAngle = _rotationAngle;
        oceanic = _oceanic;
    }
}

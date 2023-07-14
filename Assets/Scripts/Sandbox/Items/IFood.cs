using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFood
{
    float RemainingFood { get; }
    Vector3 Position { get; }

    public float Consume(float _amount);
}

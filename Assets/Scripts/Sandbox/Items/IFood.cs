using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFood
{
    float RemainingFood { get; }
    Transform Transform { get; }

    public float Consume(float _amount);
}

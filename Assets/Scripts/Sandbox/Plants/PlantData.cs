using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Plant/Plant Data")]
public class PlantData : ScriptableObject
{
    [Tooltip("growth rate, in m/s")]
    public float growthRate;

    public float maxHeight;

    public float reproductionInterval;
}

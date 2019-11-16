using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Endless Terrain/Erosion Settings")]
public class ErosionSettings : ScriptableObject
{
    public bool erode = true;
    public int seed = 0;
    [Range(2, 8)]
    public int erosionRadius = 3;
    [Range(0, 1)]
    public float inertia = .05f; // at zero, water will instally change direction. At one, it will never change direction
    public float sedimentCapacityFactor = 4;
    public float minSedimentCapacity = .01f; // used to prevent carry capacity getting too close to zero on flat terrain
    [Range(0, 1)]
    public float erodeSpeed = .3f;
    [Range(0, 1)]
    public float depositySpeed = .3f;
    [Range(0, 1)]
    public float evaporateSpeed = 0.1f;
    public float gravity = 4f;
    public int maxDropletLifetime = 30;

    public float initialWaterVolume = 1;
    public float initialSpeed = 1;

    public int numIterations = 100000;
}

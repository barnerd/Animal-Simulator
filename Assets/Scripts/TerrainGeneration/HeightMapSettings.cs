using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Endless Terrain/Height Map Settings")]
public class HeightMapSettings : ScriptableObject
{
    public NoiseMapSettings noiseSettings;

    public float heightMultiplier;
}

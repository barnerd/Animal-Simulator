using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Endless Terrain/Height Map Settings")]
public class HeightMapSettings : ScriptableObject
{
    public NoiseMapSettings noiseSettings;
    public ErosionSettings erosionSettings;

    public float heightMultiplier;

    [Header("Topographic Details")]
    public bool hasElevationLines;
    public float elevationPerMajorLine;
    public float widthOfMajorLine;
    public float elevationPerMinorLine;
    public float widthOfMinorLine;
}

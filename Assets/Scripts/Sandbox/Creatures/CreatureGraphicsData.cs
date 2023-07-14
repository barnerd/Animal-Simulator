using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Creature", menuName = "Creature/Creature Graphics Data")]
public class CreatureGraphicsData : ScriptableObject
{
    new public string name = "New Creature Graphics Data";
    public SkinnedMeshRenderer headMesh;
    public SkinnedMeshRenderer bodyMesh;
    public float meshScale;

    public RuntimeAnimatorController animatorController;
    public Avatar avatar;
    public GameObject boneHierarchy;

    [Header("Character Controller")]
    public Vector3 characterControllerCenter;
    public float characterControllerRadius;
    public float characterControllerHeight;

    [Header("UI - Meters")]
    public float metersHeight;

    [Header("Equipment Display")] 
    public float equipmentScale;
}
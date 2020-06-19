using UnityEngine;

[CreateAssetMenu(fileName = "New Creature", menuName = "Creature/Creature Data")]
public class CreatureData : ScriptableObject
{
    new public string name = "New Creature";
    public SkinnedMeshRenderer headMesh;
    public SkinnedMeshRenderer bodyMesh;

    public RuntimeAnimatorController animator;
    public Avatar avatar;
    public GameObject boneHierarchy;

    [Header("Camera Controller")]
    public Vector3 cameraOffset;

    [Header("Character Controller")]
    public Vector3 characterControllerCenter;
    public float characterControllerRadius;
    public float characterControllerHeight;

    [Header("UI - Meters")]
    public float metersHeight;

    [Header("Equipment Display")] 
    public float equipmentScale;
}
using UnityEngine;

[CreateAssetMenu(fileName = "New Creature", menuName = "Creature/Creature")]
public class CreatureData : ScriptableObject
{
    new public string name = "New Creature";
    public GameObject modelData;

    public RuntimeAnimatorController animator;

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreatureGFX : MonoBehaviour
{
    public SkinnedMeshRenderer bodyMesh;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetBlendShapes(EquipmentMeshBlendShape[] _blendShapes, float _weight)
    {
        float weight = Mathf.Clamp01(_weight) * 100;

        for (int i = 0; i < _blendShapes.Length; i++)
        {
            bodyMesh.SetBlendShapeWeight(_blendShapes[i].blendShapeIndex, weight);
        }
    }

    public void AddEquipmentGraphics(EquipmentData _equipment)
    {
        // Create new object for this equipment
        SkinnedMeshRenderer equipment = Instantiate<SkinnedMeshRenderer>(_equipment.skinnedMesh, bodyMesh.transform);

        // Set item
        equipment.gameObject.AddComponent<ItemGFX>().item = _equipment;

        // Set bones
        equipment.bones = bodyMesh.bones;
        equipment.rootBone = bodyMesh.rootBone;
    }

    public void RemoveEquipmentGraphics(EquipmentData _equipment)
    {
        // loop through children
        foreach (var child in bodyMesh.GetComponentsInChildren<ItemGFX>())
        {
            if (child.item == _equipment)
            {
                child.enabled = false;
                Destroy(child.gameObject);
            }
        }
    }

    public static void RetargetBones(SkinnedMeshRenderer _skinnedMesh, Dictionary<string, Transform> _boneMap)
    {
        Transform[] newBones = new Transform[_skinnedMesh.bones.Length];

        for (int i = 0; i < _skinnedMesh.bones.Length; i++)
        {
            Transform bone = _skinnedMesh.bones[i];
            if (!_boneMap.TryGetValue(bone.name, out newBones[i]))
            {
                Debug.Log("Unable to map bone \"" + bone.name + "\" to target skeleton.");
                break;
            }
        }
        _skinnedMesh.bones = newBones;

        Transform newRootBone;
        if (!_boneMap.TryGetValue(_skinnedMesh.rootBone.name, out newRootBone))
        {
            Debug.Log("Unable to map bone \"" + _skinnedMesh.rootBone.name + "\" to target skeleton.");
        }
        _skinnedMesh.rootBone = newRootBone;
    }
}

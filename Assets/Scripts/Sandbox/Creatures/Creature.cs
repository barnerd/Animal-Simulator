using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Creature : MonoBehaviour
{
    public CreatureData creatureData;
    public GameObject gfxObject;

    public InputController currentController;

    public float nextTimeForAIUpdate;

    //public Interactable focus;

    // Start is called before the first frame update
    void Start()
    {
        currentController.Initialize(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        currentController.ProcessInput(this.gameObject);
    }

    public bool Interact(Interactable _focus)
    {
        return _focus.Interact(this.gameObject);
    }

    public virtual void Die()
    {
        Debug.Log(name + " dies. Sad.");
    }

    public static GameObject Create(GameObject _prefab, CreatureData _data, Vector3 _position, Transform _parent = null)
    {
        GameObject creature = Instantiate(_prefab, _position, Quaternion.identity, _parent);

        creature.GetComponent<Creature>().creatureData = _data;

        GameObject gfx = creature.GetComponent<Creature>().gfxObject;

        // create skinned meshes for body, and for head if it's there
        // get skeleton
        GameObject skeleton = Instantiate<GameObject>(_data.boneHierarchy, gfx.transform);
        skeleton.name = "Humanoid";
        Transform[] bones = skeleton.GetComponentsInChildren<Transform>();

        Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
        foreach (Transform bone in bones)
            boneMap[bone.name] = bone;

        SkinnedMeshRenderer body = Instantiate<SkinnedMeshRenderer>(_data.bodyMesh, gfx.transform);
        body.name = "body";
        CreatureGFX.RetargetBones(body, boneMap);

        if (_data.headMesh != null)
        {
            SkinnedMeshRenderer head = Instantiate<SkinnedMeshRenderer>(_data.headMesh, gfx.transform);
            head.name = "head";
            CreatureGFX.RetargetBones(head, boneMap);
        }

        Animator gfxAnimator = gfx.GetComponent<Animator>();
        gfxAnimator.runtimeAnimatorController = _data.animator;
        gfxAnimator.avatar = _data.avatar;

        CreatureGFX creatureGFX = gfx.GetComponent<CreatureGFX>();
        creatureGFX.bodyMesh = body;

        CharacterController characterController = creature.GetComponent<CharacterController>();
        characterController.center = _data.characterControllerCenter;
        characterController.radius = _data.characterControllerRadius;
        characterController.height = _data.characterControllerHeight;

        creature.transform.Find("Meters").transform.localPosition = new Vector3(0, _data.metersHeight, 0);

        return creature;
    }

#if UNITY_EDITOR
    [ContextMenu("Load from CreatureData")]
    void LoadCreatureData()
    {
        if (creatureData != null)
        {
            CharacterController characterController = GetComponent<CharacterController>();
            characterController.center = creatureData.characterControllerCenter;
            characterController.radius = creatureData.characterControllerRadius;
            characterController.height = creatureData.characterControllerHeight;

            transform.Find("Meters").transform.localPosition = new Vector3(0, creatureData.metersHeight, 0);

            Instantiate<SkinnedMeshRenderer>(creatureData.bodyMesh, gfxObject.transform);
            if (creatureData.headMesh != null)
                Instantiate<SkinnedMeshRenderer>(creatureData.headMesh, gfxObject.transform);
        }
        else
        {
            Debug.LogError("Please assign a CreatureData object to load this data from.");
        }
    }

    [ContextMenu("Save to CreatureData")]
    void SaveCreatureData()
    {
        if (creatureData != null)
        {
            CharacterController characterController = GetComponent<CharacterController>();
            creatureData.characterControllerCenter = characterController.center;
            creatureData.characterControllerRadius = characterController.radius;
            creatureData.characterControllerHeight = characterController.height;

            creatureData.metersHeight = transform.Find("Meters").transform.localPosition.y;
        }
        else
        {
            Debug.LogError("Please assign a CreatureData object to save this data to.");
        }
    }

    [ContextMenu("Clear CreatureData")]
    void ClearCreatureData()
    {
        CreatureData data = GetComponent<Creature>().creatureData;

        if (data != null)
        {
            for (int i = gfxObject.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = gfxObject.transform.GetChild(i);
                DestroyImmediate(child.gameObject);
            }

            GetComponent<Creature>().creatureData = null;
        }
        else
        {
            Debug.LogError("Please assign a CreatureData object to clear the correct data.");
        }
    }
#endif
}

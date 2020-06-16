using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Creature : MonoBehaviour
{
    public CreatureData creatureData;

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
        Debug.Log(name + " dies.");
    }

    public static GameObject Create(GameObject _prefab, CreatureData _data, Vector3 _position, Transform _parent = null)
    {
        GameObject creature = Instantiate(_prefab, _position, Quaternion.identity, _parent);

        creature.GetComponent<Creature>().creatureData = _data;

        GameObject gfx = Instantiate(_data.modelData, Vector3.zero, Quaternion.identity, creature.transform);
        gfx.name = "GFX";
        gfx.transform.localPosition = Vector3.zero;

        Animator gfxAnimator;
        if (gfx.TryGetComponent(typeof(Animator), out Component animatorComponent))
        {
            gfxAnimator = (Animator)animatorComponent;
        }
        else
        {
            gfxAnimator = gfx.AddComponent<Animator>();
        }
        gfxAnimator.runtimeAnimatorController = _data.animator;

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
        CreatureData data = GetComponent<Creature>().creatureData;
        if (data != null)
        {
            CharacterController characterController = GetComponent<CharacterController>();
            characterController.center = data.characterControllerCenter;
            characterController.radius = data.characterControllerRadius;
            characterController.height = data.characterControllerHeight;

            transform.Find("Meters").transform.localPosition = new Vector3(0, data.metersHeight, 0);

            // TODO: only add a new Prefab if one doesn't already exist
            GameObject gfx = (GameObject)PrefabUtility.InstantiatePrefab(data.modelData);
            gfx.transform.SetParent(transform, false);
        }
        else
        {
            Debug.LogError("Please assign a CreatureData object to load this data from.");
        }
    }

    [ContextMenu("Save to CreatureData")]
    void SaveCreatureData()
    {
        CreatureData data = GetComponent<Creature>().creatureData;
        if (data != null)
        {
            CharacterController characterController = GetComponent<CharacterController>();
            data.characterControllerCenter = characterController.center;
            data.characterControllerRadius = characterController.radius;
            data.characterControllerHeight = characterController.height;

            data.metersHeight = transform.Find("Meters").transform.localPosition.y;
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
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);

                if (data.modelData == PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject))
                {
                    DestroyImmediate(child.gameObject);
                }
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

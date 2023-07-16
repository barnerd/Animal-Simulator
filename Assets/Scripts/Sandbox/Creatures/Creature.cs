using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Creature : MonoBehaviour
{
    public enum Sex
    {
        male,
        female
    }

    public Sex sex;

    public CreatureLogicSM logicSM;
    public CreatureData creatureData;
    public GameObject gfxObject;

    public InputController currentController;

    public float nextTimeForAIUpdate;

    private List<Interactable> nearbyInteractables;
    private Interactable closestInteractable;

    //public Interactable focus;

    public IFood consumptionTarget;
    public float moveToTransformClosingDistance;
    public State<Creature> moveToTransformNextState;
    public Transform moveToTransformTarget;
    private List<IFood> nearbyFood;

    void Awake()
    {
        logicSM = new CreatureLogicSM();

        nearbyInteractables = new List<Interactable>();
        nearbyFood = new List<IFood>();
    }

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

    public void PrepareMoveToTransform(Transform _target, float _closingDistance, State<Creature> _nextState)
    {
        moveToTransformTarget = _target;
        moveToTransformClosingDistance = _closingDistance;
        moveToTransformNextState = _nextState;
    }

    public void AddNearbyFood(IFood _food)
    {
        if (_food != null && !nearbyFood.Contains(_food))
        {
            nearbyFood.Add(_food);
        }
    }

    public IFood FindClosestFood(float _minFood)
    {
        IFood newClosestFood = null;
        float distance, closestDistance = 99999f;

        foreach (var food in nearbyFood)
        {
            if (food != null)
            {
                if (food.RemainingFood > _minFood)
                {
                    distance = Vector3.Distance(transform.position, food.Transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        newClosestFood = food;
                    }
                }
            }
        }

        return newClosestFood;
    }

    public void Blink()
    {
        GetComponentInChildren<Sight>().GetComponent<SphereCollider>().enabled = false;
        Debug.Log("Blink: " + name + " is blinking");
        GetComponentInChildren<Sight>().GetComponent<SphereCollider>().enabled = true;
    }

    public void Eat(IFood _food)
    {
        CreatureAttributes _creatureAttributes = GetComponent<CreatureAttributes>();
        if (_food != null && _creatureAttributes.GetHungerPercent() < 1.0f)
        {

            float eatAmount = Mathf.Min(100 * _creatureAttributes.GetHungerPercent(), Time.deltaTime * creatureData.eatRate);
            eatAmount = _food.Consume(eatAmount);

            _creatureAttributes.ChangeHunger(eatAmount);
        }
    }

    public void Drink(WaterBody _water)
    {

    }

    public void AddInteractable(Interactable _focus)
    {
        nearbyInteractables.Add(_focus);
        FindClosestInteractable();
    }

    public void RemoveInteractable(Interactable _focus)
    {
        nearbyInteractables.Remove(_focus);
        FindClosestInteractable();
    }

    public void FindClosestInteractable()
    {
        Interactable newClosestInteractable = null;
        float distance, closestDistance = 99999f;

        foreach (var interactable in nearbyInteractables)
        {
            distance = Vector3.Distance(transform.position, interactable.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                newClosestInteractable = interactable;
            }
        }

        if (closestInteractable != newClosestInteractable)
        {
            // remove closest's label
            if (closestInteractable != null)
                closestInteractable.HideLabel();

            closestInteractable = newClosestInteractable;

            // show closest's label
            if (closestInteractable != null)
                closestInteractable.ShowLabel();
        }
    }

    public bool Interact()
    {
        return Interact(closestInteractable);
    }

    public bool Interact(Interactable _focus)
    {
        return (_focus != null) ? _focus.Interact(this.gameObject) : false;
    }

    public virtual void Die()
    {
        Debug.Log(name + " dies. Sad.");
    }

    public static GameObject Create(GameObject _prefab, CreatureData _data, Vector3 _position, Sex _sex = Sex.female, Transform _parent = null)
    {
        GameObject creature = Instantiate(_prefab, _position, Quaternion.identity, _parent);

        creature.GetComponent<Creature>().creatureData = _data;

        creature.GetComponent<Creature>().sex = _sex;

        GameObject gfx = creature.GetComponent<Creature>().gfxObject;
        CreatureGraphicsData graphicsData = (_sex == Sex.female) ? _data.femaleGraphcisData : _data.maleGraphcisData;

        // create skinned meshes for body, and for head if it's there
        // get skeleton
        GameObject skeleton = Instantiate<GameObject>(graphicsData.boneHierarchy, gfx.transform);
        skeleton.name = "Humanoid";
        Transform[] bones = skeleton.GetComponentsInChildren<Transform>();

        Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
        foreach (Transform bone in bones)
            boneMap[bone.name] = bone;

        SkinnedMeshRenderer body = Instantiate<SkinnedMeshRenderer>(graphicsData.bodyMesh, gfx.transform);
        body.name = "body";
        CreatureGFX.RetargetBones(body, boneMap);

        if (graphicsData.headMesh != null)
        {
            SkinnedMeshRenderer head = Instantiate<SkinnedMeshRenderer>(graphicsData.headMesh, gfx.transform);
            head.name = "head";
            CreatureGFX.RetargetBones(head, boneMap);
        }

        Animator gfxAnimator = gfx.GetComponent<Animator>();
        gfxAnimator.runtimeAnimatorController = Instantiate(graphicsData.animatorController);
        gfxAnimator.avatar = graphicsData.avatar;

        CreatureGFX creatureGFX = gfx.GetComponent<CreatureGFX>();
        creatureGFX.bodyMesh = body;
        creatureGFX.transform.localScale = graphicsData.meshScale * Vector3.one;

        // TODO: move these values over to the new capsule collider
        /* CharacterController characterController = creature.GetComponent<CharacterController>();
        characterController.center = _data.characterControllerCenter;
        characterController.radius = _data.characterControllerRadius;
        characterController.height = _data.characterControllerHeight;*/

        creature.transform.Find("Meters").transform.localPosition = new Vector3(0, graphicsData.metersHeight, 0);

        return creature;
    }

    /*#if UNITY_EDITOR
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
    #endif*/
}

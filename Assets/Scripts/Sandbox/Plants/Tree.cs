using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public TreeData treeData;

    public bool showBuds = true;
    public bool showWater = false;
    public bool showEnergy = false;
    public bool showDirections = false;

    int seed;
    Vector3 dimensions;

    public float minTipLengthForBud;
    public float branchAngle; // angle around this branch to place the bud. i.e. 180 is alternating on top and bottom of branch
    public float branchingAngle; // angle from branch that this branch 
    public float minTipDistanceForBranching;

    public float branchGrowthRate;
    public float branchRadiusGrowthRate;

    public float energyRequiredToGrowBranch;
    public float energyRequiredToGrowFlower;
    public float energyRequiredToBranch;
    public float photosynthesisRate;

    public float minEnergyStored;

    public float maxWaterTransfer;
    public float maxEnergyTransfer;
    public float maxWaterCapacity;
    public float maxEnergyCapacity;

    public float waterAbsorbtionRate;

    public List<PlantBud> buds;

    public Sun sunlight;

    // Start is called before the first frame update
    void Start()
    {
        seed = 1;
        //Random.InitState(seed);

        float width = GetRandomValue(treeData.minDimensions.x, treeData.maxDimensions.x);
        float height = GetRandomValue(treeData.minDimensions.y, treeData.maxDimensions.y);
        float length = GetRandomValue(treeData.minDimensions.z, treeData.maxDimensions.z);

        dimensions = new Vector3(width, height, length);

        minTipLengthForBud = 1f;
        minTipDistanceForBranching = 5f;
        branchingAngle = 30f * Mathf.Deg2Rad;
        branchAngle = 137.5f * Mathf.Deg2Rad;

        // Initialize branches
        energyRequiredToGrowBranch = .5f;
        energyRequiredToGrowFlower = 100f;
        energyRequiredToBranch = 33f;
        photosynthesisRate = 5f; // based on surface area of leaves
        branchGrowthRate = .01f;
        branchRadiusGrowthRate = .001f;
        minEnergyStored = 25f;
        maxWaterTransfer = .31f;
        maxEnergyTransfer = .5f;
        maxWaterCapacity = 5f; // units per 1m of branch, increases with radius
        maxEnergyCapacity = 300f;
        waterAbsorbtionRate = 5f;

        PlantBud rootTip = new PlantBud(this, Vector3.zero, 0f, Vector3.down, BudType.RootTip);
        PlantBud root = new PlantBud(this, Vector3.zero, 0f, Vector3.zero, BudType.Root, rootTip);
        PlantBud tip = new PlantBud(this, Vector3.zero, 0f, Vector3.up, BudType.Tip, root);
        root.currentEnergyLevel += energyRequiredToBranch; // TODO: remove. This is because seeds aren't programmed yet and trees need a starting point. Later, seeds will come with an initial energy level
        buds.Add(root);
        buds.Add(rootTip);
        buds.Add(tip);
    }

    // Update is called once per frame
    void Update()
    {
        Grow(10f * Time.deltaTime);

        float totalWater = GetTotalWater();
        float totalEnergy = GetTotalEnergy();
        Debug.Log("Water / Bud: " + totalWater / buds.Count + ", Total Water: " + totalWater + ", Energy / Bud: " + totalEnergy / buds.Count + ", Total Energy: " + totalEnergy);

    }

    float GetTotalWater()
    {
        float total = 0f;

        foreach (var bud in buds)
        {
            total += bud.currentWaterLevel;
        }

        return total;
    }

    float GetTotalEnergy()
    {
        float total = 0f;

        foreach (var bud in buds)
        {
            total += bud.currentEnergyLevel;
        }

        return total;
    }

    float GetRandomValue(float min, float max)
    {
        float delta = max - min;
        return Random.value * delta + min;
    }

    void Grow(float _length)
    {
        for (int i = buds.Count - 1; i >= 0; i--)
        {
            buds[i].Grow(_length);
        }
    }

    void ChangeDormancy(bool _dormant = true)
    {
        foreach (var bud in buds)
        {
            bud.dormant = _dormant;
        }
    }

    public static GameObject Create(GameObject _prefab, TreeData _treeData, Vector3 _position, Transform _parent = null)
    {
        GameObject tree = Instantiate(_prefab, _position, Quaternion.identity, _parent);
        tree.name = _treeData.name;

        tree.GetComponent<Tree>().treeData = _treeData;

        return tree;
    }

    void OnDrawGizmos()
    {
        // Draw Cube Outline
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, dimensions.y / 2f, 0), dimensions);

        if (showBuds)
        {
            // Draw Branch Skeleton
            foreach (var bud in buds)
            {
                DrawGizmos(bud);
            }
        }
    }

    void DrawGizmos(PlantBud _bud)
    {
        if (_bud.parent != null)
        {
            Gizmos.color = new Color(160f / 255f, 82f / 255f, 45f / 255f);
            Gizmos.DrawLine(transform.position + _bud.position, transform.position + _bud.parent.position);
        }

        // Show direction of bud
        if (showDirections)
        {
        Gizmos.color = new Color(1f, 0f, 0f);
        Gizmos.DrawLine(transform.position + _bud.position, transform.position + _bud.position + _bud.direction.normalized / 3f);
        }

        // Draw Bud Type
        switch (_bud.type)
        {
            case BudType.Tip:
                Gizmos.color = Color.blue;
                break;
            case BudType.Flower:
                Gizmos.color = Color.yellow;
                break;
            case BudType.Leaf:
                Gizmos.color = Color.green;
                break;
            default:
                Gizmos.color = Color.black;
                break;
        }
        if (!showWater && !showEnergy)
        {
            Gizmos.DrawSphere(transform.position + _bud.position, .05f);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position + _bud.position, .06f);
        }

        // Draw Water Level
        if (showWater && !showEnergy)
        {
            Gizmos.color = new Color(0f, 0f, _bud.currentWaterLevel / _bud.MaxWaterCapacity);
            Gizmos.DrawSphere(transform.position + _bud.position, .05f);
        }

        // Draw Energy Level
        if (showEnergy)
        {
            Gizmos.color = new Color(_bud.currentEnergyLevel / _bud.MaxEnergyCapacity, _bud.currentEnergyLevel / _bud.MaxEnergyCapacity, 0f);
            Gizmos.DrawSphere(transform.position + _bud.position, .05f);
        }
    }

    void OnDrawGizmosSelected()
    {
    }
}

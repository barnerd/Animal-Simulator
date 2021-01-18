using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BudType { Tip, Fork, Leaf, Spine, Thorn, Flower, Root, RootTip }

[System.Serializable]
public class PlantBud
{
    const float PI2 = Mathf.PI * 2f;

    public Tree tree;

    public Vector3 position;
    public Vector3 direction;

    public float lengthFromRoot;

    public float branchAngle;
    public float branchRadius;

    public BudType type;

    [Header("Energy Flow")]
    public float currentWaterLevel;
    public float MaxWaterCapacity
    {
        get
        {
            if (type == BudType.Root || type == BudType.RootTip)
                return Mathf.PI * branchRadius * branchRadius * tree.maxWaterCapacity; // from equation of area
            else
                return PI2 * branchRadius * tree.maxWaterCapacity; // from equation of circumference
        }
    }
    public float currentEnergyLevel;
    public float MaxEnergyCapacity
    {
        get
        {
            if (type == BudType.Root || type == BudType.RootTip)
                return Mathf.PI * branchRadius * branchRadius * tree.maxEnergyCapacity; // from equation of area
            else
                return PI2 * branchRadius * tree.maxEnergyCapacity; // from equation of circumference
        }
    }

    public float MaxWaterTransfer
    {
        get
        {
            if (type == BudType.Root || type == BudType.RootTip)
                return Mathf.PI * branchRadius * branchRadius * tree.maxWaterTransfer; // from equation of area
            else
                return PI2 * branchRadius * tree.maxWaterTransfer; // from equation of circumference
        }
    }

    public float MaxEnergyTransfer
    {
        get
        {
            if (type == BudType.Root || type == BudType.RootTip)
                return Mathf.PI * branchRadius * branchRadius * tree.maxEnergyTransfer; // from equation of area
            else
                return PI2 * branchRadius * tree.maxEnergyTransfer; // from equation of circumference
        }
    }

    public bool dormant;
    public bool dead;

    public PlantBud parent;

    [Header("Mesh Values")]
    public List<int> vertexIndices;
    public int tipIndex;

    public PlantBud(Tree _tree, Vector3 _pos, float _angle, Vector3 _dir, BudType _type, PlantBud _parent = null)
    {
        tree = _tree;

        position = _pos;
        branchAngle = _angle;
        direction = _dir;

        branchRadius = .01f;
        type = _type;

        dormant = false;
        dead = false;

        parent = _parent;

        lengthFromRoot = (_parent == null) ? 0f : _parent.lengthFromRoot + Vector3.Distance(position, _parent.position);

        vertexIndices = new List<int>();
        tipIndex = -1;
    }

    public void Grow(float _deltaTime)
    {
        // no time has passed, skip growth
        if (Mathf.Abs(_deltaTime) < 1e-8f)
        {
            return;
        }

        float remaining, delta, percentage;

        // take water & energy from parent
        if (type != BudType.RootTip)
        {
            float deltaWater = Mathf.Clamp(MaxWaterCapacity - currentWaterLevel, 0, MaxWaterTransfer * _deltaTime);
            float deltaEnergy = MaxEnergyTransfer * _deltaTime;
            if (parent != null)
            {
                deltaWater = parent.ChangeResource(-deltaWater, ref currentWaterLevel, MaxWaterCapacity);
                ChangeResource(deltaWater, ref currentWaterLevel, MaxWaterCapacity);

                remaining = parent.ChangeResource(-deltaEnergy, ref currentEnergyLevel, MaxEnergyCapacity);
                ChangeResource(deltaEnergy - remaining, ref currentEnergyLevel, MaxEnergyCapacity);
            }
        }

        // use energy to do bud function
        switch (type)
        {
            case BudType.Tip:
                // consume energy and grow in length
                GrowTip(_deltaTime);

                // if tip has enough length, and right time of year (buds show up in the fall for trees), add bud
                float tipLength = Vector3.Distance(position, parent.position);
                if (tipLength > tree.minTipLengthForBud)
                {
                    CreateBud();
                }
                break;
            case BudType.Leaf:
                // evaporate water
                // TODO: check temperature
                // water lose increases with temperature
                // ChangeWater(rate * temperature);

                // consume water and produce energy
                percentage = 1f; // TODO: check sunlight intensity. percent of photosynthesis increases with intensity and decreases with temperature over a threshold
                // from equation of photosynthesis, 6 waters create 1 energy
                delta = -6f * tree.photosynthesisRate * _deltaTime * percentage;
                remaining = ChangeResource(delta, ref currentWaterLevel, MaxWaterCapacity);
                percentage *= (delta - remaining) / delta;
                ChangeResource(tree.photosynthesisRate * percentage * _deltaTime, ref currentEnergyLevel, MaxEnergyCapacity);

                // if bud has enough energy, grow tip.
                // TODO: move this section to when a bud un-dorments in the spring and can change types
                if (currentEnergyLevel > tree.energyRequiredToBranch)
                {
                    type = BudType.Fork;
                    PlantBud newBud = new PlantBud(tree, position, branchAngle, direction, BudType.Tip, this);
                    tree.buds.Add(newBud);

                    newBud.ChangeResource(tree.energyRequiredToBranch, ref currentEnergyLevel, MaxEnergyCapacity);
                    ChangeResource(-tree.energyRequiredToBranch, ref currentEnergyLevel, MaxEnergyCapacity);
                }
                break;
            case BudType.Flower:
                // consume a lot of energy and produce fruit
                break;
            case BudType.Root:
                // absorb water
                ChangeResource(Mathf.PI * branchRadius * branchRadius * tree.waterAbsorbtionRate * _deltaTime, ref currentWaterLevel, MaxWaterCapacity);
                break;
            case BudType.RootTip:
                // consume energy and grow in length
                GrowTip(_deltaTime);

                // absorb water
                ChangeResource(tree.waterAbsorbtionRate * _deltaTime, ref currentWaterLevel, MaxWaterCapacity);
                break;
        }

        // use energy to increase radius
        if (type != BudType.Tip && type != BudType.RootTip)
        {
            // from the equation of circumference
            delta = -PI2 * branchRadius * tree.energyRequiredToGrowBranch * _deltaTime;
            remaining = ChangeResource(delta, ref currentEnergyLevel, MaxEnergyCapacity);
            percentage = (delta - remaining) / delta;
            branchRadius += tree.branchRadiusGrowthRate * _deltaTime * percentage;
        }

        // give energy to parent
        delta = Mathf.Clamp(currentEnergyLevel - tree.minEnergyStored, 0, MaxEnergyTransfer * _deltaTime);
        if (parent != null)
        {
            parent.ChangeResource(delta, ref currentEnergyLevel, MaxEnergyCapacity);
            ChangeResource(-delta, ref currentEnergyLevel, MaxEnergyCapacity);
        }
    }

    public float ChangeResource(float _delta, ref float _currentLevel, float _maxLevel)
    {
        _currentLevel += _delta;

        float remaining = 0f;

        if (_currentLevel > _maxLevel)
        {
            remaining = _currentLevel - _maxLevel;
            _currentLevel = _maxLevel;
        }
        else if (_currentLevel < 0f)
        {
            remaining = _currentLevel;
            _currentLevel = 0f;
        }

        return remaining;
    }

    void GrowTip(float _deltaTime)
    {
        float delta, remaining, percentage;

        // consume energy and grow in length
        delta = -tree.energyRequiredToGrowBranch * _deltaTime;
        remaining = ChangeResource(delta, ref currentEnergyLevel, MaxEnergyCapacity);
        percentage = (delta - remaining) / delta;
        position += direction * percentage * tree.branchGrowthRate * _deltaTime;

        lengthFromRoot = (parent == null) ? 0f : parent.lengthFromRoot + Vector3.Distance(position, parent.position);
    }

    void CreateBud()
    {
        // Choose new bud's position
        Vector3 newPosition = position;

        // Choose new bud's angle around the branch
        // can be opposite, alternating, or at a rotation angle
        // TODO: add other types of branch angles
        float newBranchAngle = parent.branchAngle;
        newBranchAngle += tree.branchAngle;
        newBranchAngle %= PI2;

        // Choose new bud's direction: branching angle rotated around by branchAngle
        //Quaternion rotationAroundBranch = Quaternion.AngleAxis(newBranchAngle * Mathf.Rad2Deg, tip.direction);
        //Quaternion branchingRotation = Quaternion.AngleAxis(tree.branchingAngle * Mathf.Rad2Deg, Vector3.Cross(Vector3.up, tip.direction));
        //Vector3 newDirection = rotationAroundBranch * (branchingRotation * tip.direction);

        Vector3 newDirection = Quaternion.Euler(0, newBranchAngle * Mathf.Rad2Deg, tree.branchingAngle * Mathf.Rad2Deg) * direction;

        PlantBud newBud = new PlantBud(tree, newPosition, newBranchAngle, newDirection, BudType.Leaf, parent);
        parent = newBud;
        tree.buds.Add(newBud);
        // TODO: choose new direction for tip
    }
}

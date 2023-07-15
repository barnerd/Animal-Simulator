using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IFood
{
    public PlantData plantData;
    protected float height;

    protected float remainingFood;
    public float RemainingFood { get { return remainingFood; } }
    public Transform Transform { get { return transform; } }

    protected float nextTimeForReproduction;

    // Start is called before the first frame update
    void Start()
    {
        nextTimeForReproduction = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (height < plantData.maxHeight)
        {
            GrowPlant(Time.deltaTime * plantData.growthRate);
        }

        // TODO: Move this out of Update. Maybe into weather? maybe a state machine for plants, triggered by weather
        // check conditions to create seeds
        if (Time.time > nextTimeForReproduction && height >= plantData.maxHeight)
        {
            nextTimeForReproduction = Time.time + plantData.reproductionInterval;

            Seed();
        }
    }

    public void GrowPlant(float _growthAmount)
    {
        height += _growthAmount;
        if (height > plantData.maxHeight)
        {
            height = plantData.maxHeight;
        }
    }

    public virtual float Consume(float _amount)
    {
        return _amount;
    }

    public virtual void Seed() { }
}

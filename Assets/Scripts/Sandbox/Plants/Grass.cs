using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Plant
{
    public Transform grassMesh;
    public float minEatHeight = 0.2f;
    public float density = 8f; // "pounds" per meter
    public float childRadius;

    private int numMisses;
    private int maxMisses = 3;

    public LayerMask Plants;

    void Awake()
    {
        height = 0f;
        numMisses = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        remainingFood = (height - minEatHeight) * density;
        nextTimeForReproduction = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (height < plantData.maxHeight)
        {
            GrowPlant(Time.deltaTime * plantData.growthRate);
            remainingFood = (height - minEatHeight) * density;
            UpdateGrassHeight();
        }

        // TODO: Move this out of Update. Maybe into weather? maybe a state machine for plants, triggered by weather
        // check conditions to create seeds
        if (Time.time > nextTimeForReproduction && height >= plantData.maxHeight)
        {
            nextTimeForReproduction = Time.time + plantData.reproductionInterval;

            Seed();
        }
    }

    private void UpdateGrassHeight()
    {
        grassMesh.localScale = new Vector3(grassMesh.localScale.x, grassMesh.localScale.y, 100f * height);
    }

    public override float Consume(float _amount)
    {
        float amountConsumed = Mathf.Max(0, Mathf.Min(RemainingFood, _amount));

        height -= amountConsumed / density;
        remainingFood -= amountConsumed;
        UpdateGrassHeight();

        return amountConsumed;
    }

    public override void Seed()
    {
        if (numMisses <= maxMisses)
        {
            float x, z;
            // set random position
            x = Random.Range(-childRadius, childRadius);
            z = Random.Range(-childRadius, childRadius);
            Vector3 _position = transform.position + new Vector3(x, -0.01f, z);

            bool isSurrounded = Physics.CheckSphere(_position, childRadius / 2, Plants, QueryTriggerInteraction.Collide);

            if (!isSurrounded)
            {
                numMisses = 0;
                GameObject newGrass = Instantiate(gameObject, transform.parent);
                newGrass.name = "Grass";

                newGrass.transform.position = _position;

                newGrass.transform.Rotate(new Vector3(0, 0, Random.Range(-180f, 180f)));
            }
            else
            {
                numMisses++;
            }
        }
    }
}

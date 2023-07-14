using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCreation : MonoBehaviour
{
    public int numPlants;

    public Ground ground;

    [Header("Plants")]
    public GameObject[] plantPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        float x, z;

        for (int i = 0; i < numPlants; i++)
        {
            GameObject plant = Instantiate(plantPrefabs[Random.Range(0, plantPrefabs.Length)]);
            plant.name = "Grass";

            // set random position
            x = Random.Range(ground.size * 3 / 8f, ground.size * 5 / 8f);
            z = Random.Range(ground.size * 3 / 8f, ground.size * 5 / 8f);
            x = Random.Range(0, ground.size);
            z = Random.Range(0, ground.size);
            plant.transform.position = new Vector3(x, ground.GetHeightAtXZ(x, z) - 0.1f, z);

            plant.transform.Rotate(new Vector3(0, 0, Random.Range(-180f, 180f)));

            plant.transform.parent = transform;
        }
    }
}

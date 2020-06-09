using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreation : MonoBehaviour
{
    public int numItems;
    public Ground ground;

    public Material mat;

    public Item[] items;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numItems; i++)
        {
            GameObject item = ItemPickup.CreateItemPickup(items[Random.Range(0, items.Length)]);

            // set random position
            float x = Random.Range(0f, ground.size);
            float z = Random.Range(0f, ground.size);
            item.transform.position = new Vector3(x, ground.GetHeightAtXZ(x, z) + 2f, z);

            item.transform.parent = this.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

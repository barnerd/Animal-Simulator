using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreation : MonoBehaviour
{
    public int numItems;

    public Ground ground;

    public GameObject itemPickupPrefab;

    [Header("Items")]
    public ItemData[] items;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numItems; i++)
        {
            // set random position
            float x = Random.Range(0f, ground.size);
            float z = Random.Range(0f, ground.size);
            Vector3 position = new Vector3(x, ground.GetHeightAtXZ(x, z) + 2f, z);

            GameObject item = ItemPickup.Create(itemPickupPrefab, items[Random.Range(0, items.Length)], position, transform);

            item.transform.parent = this.transform;
        }
    }
}

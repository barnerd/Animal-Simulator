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
            CreateItem(i);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private GameObject CreateItem(int num)
    {
        string name = "Item " + (num + 1);
        GameObject item = new GameObject(name);
        item.transform.parent = this.transform;

        // add body
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = "GFX";
        box.transform.parent = item.transform;
        box.transform.localScale = new Vector3(.2f, .2f, .2f);
        box.GetComponent<MeshRenderer>().sharedMaterial = mat;

        // set random position
        float x = Random.Range(0f, ground.size);
        float z = Random.Range(0f, ground.size);
        item.transform.position = new Vector3(x, ground.GetHeightAtXZ(x, z) + .2f, z);

        Rigidbody itemRB = item.AddComponent<Rigidbody>();
        itemRB.collisionDetectionMode = CollisionDetectionMode.Continuous;

        ItemPickup itemPU = item.AddComponent<ItemPickup>();
        itemPU.item = items[Random.Range(0, items.Length)];

        return item;
    }
}

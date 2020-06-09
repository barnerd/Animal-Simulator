using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    public override bool Interact(GameObject actor)
    {
        bool success = base.Interact(actor);

        success &= Pickup(actor);

        return success;
    }

    private bool Pickup(GameObject actor)
    {
        if (actor.TryGetComponent(typeof(Inventory), out Component inventory))
        {
            bool success = ((Inventory)inventory).Add(item);
            if (success)
            {
                Destroy(gameObject);

                return true;
            }
        }

        return false;
    }

    public static GameObject CreateItemPickup(Item _i)
    {
        GameObject item = new GameObject(_i.name);

        ItemPickup itemPU = item.AddComponent<ItemPickup>();
        itemPU.item = _i;

        // add mesh
        MeshFilter itemMF = item.AddComponent<MeshFilter>();
        itemMF.sharedMesh = itemPU.item.mesh;
        MeshRenderer itemMR = item.AddComponent<MeshRenderer>();
        itemMR.sharedMaterial = itemPU.item.material;

        Rigidbody itemRB = item.AddComponent<Rigidbody>();
        itemRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
        MeshCollider itemMC = item.AddComponent<MeshCollider>();
        itemMC.convex = true;

        return item;
    }
}

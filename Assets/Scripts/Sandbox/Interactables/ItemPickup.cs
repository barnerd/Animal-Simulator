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
        if (actor.TryGetComponent(typeof(InventoryManager), out Component inventory))
        {
            bool success = ((InventoryManager)inventory).Add(item);
            if (success)
            {
                Destroy(gameObject);

                return true;
            }
        }

        return false;
    }

    public static GameObject Create(GameObject _itemPickupPrefab, Item _i, Vector3 _position, Transform _parent = null)
    {
        GameObject item = Instantiate(_itemPickupPrefab, _position, Quaternion.identity, _parent);
        item.name = _i.name;

        item.GetComponent<ItemPickup>().item = _i;

        // add mesh
        item.GetComponentInChildren<MeshFilter>().sharedMesh = _i.mesh;
        item.GetComponentInChildren<MeshRenderer>().sharedMaterial = _i.material;

        return item;
    }
}

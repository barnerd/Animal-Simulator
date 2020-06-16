using UnityEngine;

public class ItemPickup : Interactable
{
    public ItemData itemData;

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
            bool success = ((InventoryManager)inventory).Add(itemData);
            if (success)
            {
                Destroy(gameObject);

                return true;
            }
        }

        return false;
    }

    public static GameObject Create(GameObject _itemPickupPrefab, ItemData _i, Vector3 _position, Transform _parent = null)
    {
        GameObject item = Instantiate(_itemPickupPrefab, _position, Quaternion.identity, _parent);
        item.name = _i.name;

        item.GetComponent<ItemPickup>().itemData = _i;

        // add mesh
        item.GetComponentInChildren<MeshFilter>().sharedMesh = _i.mesh;
        item.GetComponentInChildren<MeshRenderer>().sharedMaterial = _i.material;

        // set Collider size
        BoxCollider collider = item.GetComponent<BoxCollider>();
        collider.center = _i.colliderCenter;
        collider.size = _i.colliderSize;

        return item;
    }

#if UNITY_EDITOR
    [ContextMenu("Load from ItemData")]
    void LoadItemData()
    {
        ItemData data = GetComponent<ItemPickup>().itemData;
        if (data != null)
        {
            GameObject gfx = GetComponentInChildren<MeshFilter>().gameObject;
            gfx.GetComponent<MeshFilter>().sharedMesh = data.mesh;
            gfx.GetComponent<MeshRenderer>().sharedMaterial = data.material;

            BoxCollider collider = GetComponent<BoxCollider>();
            collider.center = data.colliderCenter;
            collider.size = data.colliderSize;
        }
        else
        {
            Debug.LogError("Please assign a ItemData object to load this data from.");
        }
    }

    [ContextMenu("Save to ItemData")]
    void SaveItemData()
    {
        ItemData data = GetComponent<ItemPickup>().itemData;
        if (data != null)
        {
            BoxCollider collider = GetComponent<BoxCollider>();
            data.colliderCenter = collider.center;
            data.colliderSize = collider.size;
        }
        else
        {
            Debug.LogError("Please assign a ItemData object to save this data to.");
        }
    }

    [ContextMenu("Clear ItemData")]
    void ClearItemData()
    {
        ItemData data = GetComponent<ItemPickup>().itemData;

        if (data != null)
        {
            GameObject gfx = GetComponentInChildren<MeshFilter>().gameObject;
            gfx.GetComponent<MeshFilter>().sharedMesh = null;
            gfx.GetComponent<MeshRenderer>().sharedMaterial = null;

            BoxCollider collider = GetComponent<BoxCollider>();
            collider.center = Vector3.zero;
            collider.size = Vector3.one;

            GetComponent<ItemPickup>().itemData = null;
        }
        else
        {
            Debug.LogError("Please assign a ItemData object to clear the correct data.");
        }
    }
#endif
}

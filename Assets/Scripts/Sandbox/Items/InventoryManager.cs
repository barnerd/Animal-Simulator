using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    // TODO: add bulk
    // TODO: add weight

    public GameEvent onInventoryChange;

    public GameObject itemPickupPrefab;

    public bool Add(Item _i)
    {
        // check bulk and weight before adding
        items.Add(_i);
        if (onInventoryChange != null)
            onInventoryChange.Raise(this);

        return true;
    }

    public void Interact(Item _i)
    {
        if (items.Contains(_i))
        {
            _i.Interact(GetComponent<Creature>());
        }
    }

    public bool Drop(Item _i)
    {
        bool success = Remove(_i);

        if (success)
        {
            // position item to the right
            Vector3 position = transform.position + transform.right * .7f;

            ItemPickup.Create(itemPickupPrefab, _i, position);
        }

        return success;
    }

    public bool Remove(Item _i)
    {
        if (items.Contains(_i))
        {
            items.Remove(_i);
            if (onInventoryChange != null)
                onInventoryChange.Raise(this);
            return true;
        }

        return false;
    }
}

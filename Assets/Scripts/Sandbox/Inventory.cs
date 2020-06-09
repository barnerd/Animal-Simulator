using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    // TODO: add bulk
    // TODO: add weight

    public GameEvent onInventoryChanged;

    public bool Add(Item _i)
    {
        // check bulk and weight before adding
        items.Add(_i);
        if (onInventoryChanged != null)
            onInventoryChanged.Raise(this);

        return true;
    }

    public bool Interact(Item _i)
    {
        if (items.Contains(_i))
        {
            return _i.Interact();
        }

        return false;
    }

    public bool Drop(Item _i)
    {
        bool success = Remove(_i);

        if (success)
        {
            GameObject item = ItemPickup.CreateItemPickup(_i);

            // position item to the right
            item.transform.position = transform.position + transform.right * .7f;
        }

        return success;
    }

    public bool Remove(Item _i)
    {
        if (items.Contains(_i))
        {
            items.Remove(_i);
            if (onInventoryChanged != null)
                onInventoryChanged.Raise(this);
            return true;
        }

        return false;
    }
}

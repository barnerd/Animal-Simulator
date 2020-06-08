using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    // TODO: add bulk
    // TODO: add weight

    public bool Add(Item _i)
    {
        // check bulk and weight before adding
        items.Add(_i);

        return true;
    }

    public bool Remove(Item _i)
    {
        if (items.Contains(_i))
        {
            items.Remove(_i);
            return true;
        }

        return false;
    }
}

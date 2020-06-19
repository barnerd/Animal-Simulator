using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<ItemData> items = new List<ItemData>();

    // TODO: add bulk
    // TODO: add weight

    public GameEvent onInventoryChange;

    public GameObject itemPickupPrefab;

    private Creature creature;
    private CreatureMotor creatureMotor;

    void Start()
    {
        creature = GetComponent<Creature>();
        creatureMotor = GetComponent<CreatureMotor>();
    }

    public bool Add(ItemData _i)
    {
        // TODO: check for casting first
        if(((EquipmentData)_i).baseClothing)
                return true;

        // TODO: check bulk and weight before adding
        items.Add(_i);
        if (onInventoryChange != null)
            onInventoryChange.Raise(this);

        return true;
    }

    public void Interact(ItemData _i)
    {
        if (items.Contains(_i))
        {
            _i.Interact(creature);
        }
    }

    public bool Drop(ItemData _i)
    {
        bool success = Remove(_i);

        if (success)
        {
            // position item to the right
            Vector3 position = transform.position + transform.right * .7f;

            creatureMotor.Drop();
            ItemPickup.Create(itemPickupPrefab, _i, position);
        }

        return success;
    }

    public bool Remove(ItemData _i)
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

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
}

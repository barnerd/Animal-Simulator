using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    public override bool Interact(GameObject actor)
    {
        bool outcome = base.Interact(actor);

        Debug.Log(actor.name + " interacts with " + this.name);

        outcome &= Pickup(actor);

        return outcome;
    }

    private bool Pickup(GameObject actor)
    {
        Debug.Log("attempting to pickup " + item.name);

        if (actor.TryGetComponent(typeof(Inventory), out Component inventory))
        {
            bool success = ((Inventory)inventory).Add(item);
            if (success)
            {
                actor.GetComponent<Creature>().RemoveFocus();
                Destroy(gameObject);

                return true;
            }
        }

        return false;
    }
}

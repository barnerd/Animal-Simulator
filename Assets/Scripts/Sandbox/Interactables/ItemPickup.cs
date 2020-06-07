using UnityEngine;

public class ItemPickup : Interactable
{
    public override bool Interact(GameObject actor)
    {
        bool outcome = base.Interact(actor);

        Debug.Log(actor.name + " interacts with " + this.name);

        outcome &= Pickup(actor);

        return outcome;
    }

    private bool Pickup(GameObject actor)
    {
        Debug.Log("attempting to pickup item");
        transform.parent = actor.transform;
        actor.GetComponent<Creature>().RemoveFocus();
        this.gameObject.SetActive(false);

        return true;
    }
}

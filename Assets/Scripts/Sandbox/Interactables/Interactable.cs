using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 2f;

    public virtual bool Interact(GameObject actor)
    {
        //Debug.Log(actor.name + " interacts with " + this.name);

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

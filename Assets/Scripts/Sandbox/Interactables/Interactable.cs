using UnityEngine;

public class Interactable : MonoBehaviour
{
    //public float radius = 2f;
    public GameObject labelPanel;

    public virtual bool Interact(GameObject actor)
    {
        //Debug.Log(actor.name + " interacts with " + this.name);

        return true;
    }

    public void ShowLabel()
    {
        if (labelPanel != null)
        {
            labelPanel.SetActive(true);
        }
    }

    public void HideLabel()
    {
        if (labelPanel != null)
        {
            labelPanel.SetActive(false);
        }
    }

    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }*/
}

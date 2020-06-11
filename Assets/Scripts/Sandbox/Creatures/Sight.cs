using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public Creature creature;

    // Start is called before the first frame update
    void Start()
    {
        if (creature == null)
            creature = transform.parent.GetComponent<Creature>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Creature>(out Creature c))
        {
            Debug.Log(creature.name + " can now see " + c + ", a creature.");
        }
        else if (other.TryGetComponent<ItemPickup>(out ItemPickup i))
        {
            Debug.Log(creature.name + " can now see " + i.item + ", an item.");
        }
        else
        {
            Debug.Log(creature.name + " can now see " + other + ", which I don't know what it is");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /*if (other.TryGetComponent<Creature>(out Creature c))
        {
            Debug.Log(creature.name + " can no longer see " + c + ", a creature.");
        }
        else if (other.TryGetComponent<ItemPickup>(out ItemPickup i))
        {
            Debug.Log(creature.name + " can no longer see " + i.item + ", an item.");
        }
        else
        {
            Debug.Log(creature.name + " can no longer see " + other + ", which I don't know what it is");
        }*/
    }

    //private void OnTriggerStay(Collider other) { }
}

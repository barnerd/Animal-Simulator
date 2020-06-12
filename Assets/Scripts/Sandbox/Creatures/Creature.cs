using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public InputController currentController;

    public float nextTimeForAIUpdate;

    //public Interactable focus;

    // Start is called before the first frame update
    void Start()
    {
        currentController.Initialize(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        currentController.ProcessInput(this.gameObject);
    }

    public bool Interact(Interactable _focus)
    {
        return _focus.Interact(this.gameObject);
    }

    public virtual void Die()
    {
        Debug.Log(name + " dies.");
    }

    public static GameObject Create(GameObject _prefab, Vector3 _position, Transform _parent = null)
    {
        GameObject creature = Instantiate(_prefab, _position, Quaternion.identity, _parent);

        return creature;
    }
}

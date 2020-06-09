using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Mesh mesh;
    public Material material;

    [Header("Inventory")]
    public float inventoryScale;
    public Vector3 inventoryPosition;
    public Quaternion inventoryRotation;

    public virtual void Interact(MonoBehaviour _actor)
    {
        Debug.Log(((Creature)_actor).name + " cannot use item: " + this.name);
    }
}

// candy cane rotation: -90, 0, 180
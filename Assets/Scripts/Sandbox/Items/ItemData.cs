using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Item")]
public class ItemData : ScriptableObject
{
    new public string name = "New Item";

    [Header("Graphics")]
    public Mesh mesh;
    public Material material;

    [Header("Box Collider")]
    public Vector3 colliderCenter;
    public Vector3 colliderSize;

    [Header("Inventory Display")]
    public float inventoryScale;
    public Vector3 inventoryPosition;
    //TODO: Get Quaternion to display correctly in editor
    public Quaternion inventoryRotation;

    public virtual void Interact(MonoBehaviour _actor)
    {
        Debug.Log(((Creature)_actor).name + " cannot use item: " + this.name);
    }
}

// candy cane rotation: -90, 0, 180
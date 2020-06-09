using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Mesh mesh;
    public Material material;

    public float inventoryScale;
    public Vector3 inventoryPosition;
    public Quaternion inventoryRotation;

    public virtual bool Interact()
    {
        Debug.Log("Cannot use item: " + this.name);
        return false;
    }
}

// candy cane rotation: -90, 0, 180
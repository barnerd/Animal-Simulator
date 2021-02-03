using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Slot", menuName = "Type/Equipment Slot")]
public class EquipmentSlot : ScriptableObject
{
    new public string name = "New Slot";

    public Sprite sprite;
}

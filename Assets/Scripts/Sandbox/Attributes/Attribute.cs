using UnityEngine;

[CreateAssetMenu(fileName = "New Attribute", menuName = "Attribute/Attribute")]
public class Attribute : ScriptableObject
{
    new public string name = "Attribute";
    public float value;
}

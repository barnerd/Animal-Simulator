using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StringVariable", menuName = "Primatives/StringVariable")]
public class StringVariable : ScriptableObject, ISerializationCallbackReceiver
{
#if UNITY_EDITOR
    [Multiline]
    public string Description = "";
#endif
    public string InitialValue;
    public string Value;

    public void SetValue(string value)
    {
        Value = value;
    }

    public void SetValue(StringVariable value)
    {
        Value = value.Value;
    }

    public void OnAfterDeserialize()
    {
        // OnAfterDeserialize is being called all the time
        // cannot save Value from editor, as it's being overridden by InitialValue
        // Value = InitialValue;
    }

    public void OnBeforeSerialize() { }
}

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariable", menuName = "Primatives/BoolVariable")]
public class BoolVariable : ScriptableObject, ISerializationCallbackReceiver
{
#if UNITY_EDITOR
    [Multiline]
    public string Description = "";
#endif
    public bool InitialValue;
    public bool Value;

    public void SetValue(bool value)
    {
        Value = value;
    }

    public void SetValue(BoolVariable value)
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

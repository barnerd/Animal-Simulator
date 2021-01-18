using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "Primatives/IntVariable")]
public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
{
#if UNITY_EDITOR
    [Multiline]
    public string Description = "";
#endif
    public int InitialValue;
    public int Value;

    public void SetValue(int value)
    {
        Value = value;
    }

    public void SetValue(IntVariable value)
    {
        Value = value.Value;
    }

    public void ApplyChange(int amount)
    {
        Value += amount;
    }

    public void ApplyChange(IntVariable amount)
    {
        Value += amount.Value;
    }

    public void OnAfterDeserialize()
    {
        // OnAfterDeserialize is being called all the time
        // cannot save Value from editor, as it's being overridden by InitialValue
        // Value = InitialValue;
    }

    public void OnBeforeSerialize() { }
}

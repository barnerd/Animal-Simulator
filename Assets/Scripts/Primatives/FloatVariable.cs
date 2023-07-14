using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariable", menuName = "Primatives/FloatVariable")]
public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
{
#if UNITY_EDITOR
    [Multiline]
    public string Description = "";
#endif
    public float InitialValue;
    public float Value;

    public void SetValue(float value)
    {
        Value = value;
    }

    public void SetValue(FloatVariable value)
    {
        Value = value.Value;
    }

    public void ApplyChange(float amount)
    {
        Value += amount;
    }

    public void ApplyChange(FloatVariable amount)
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

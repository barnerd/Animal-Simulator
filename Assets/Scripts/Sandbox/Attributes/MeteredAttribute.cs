using UnityEngine;

[System.Serializable]
public class MeteredAttribute : Attribute
{
    public float CurrentPercent { get { return currentValue / BaseValue; } }

    public GameEvent onMeteredAttributeChange;
    public GameEvent onMeteredAttribute0;

    public MeteredAttribute(float _base) : base(_base)
    {
    }
}

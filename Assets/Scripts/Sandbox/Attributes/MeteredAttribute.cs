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

    public void ChangeMeter(float _delta, CreatureAttributes _actor)
    {
        currentValue += _delta;

        if (onMeteredAttributeChange != null)
            onMeteredAttributeChange.Raise(_actor);

        if (currentValue <= 0)
        {
            if (onMeteredAttribute0 != null)
                onMeteredAttribute0.Raise(_actor);
        }
    }
}

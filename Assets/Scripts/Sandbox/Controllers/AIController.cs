using UnityEngine;

[CreateAssetMenu(fileName = "InputController", menuName = "Input Controller/AI Controller")]
public class AIController : InputController
{
    public float updateInterval = 5f;

    [Tooltip("% before searching")]
    [Range(0.0f, 1.0f)]
    public float hungerSearchThreshold;
    [Tooltip("% before searching")]
    [Range(0.0f, 1.0f)]
    public float thirstSearchThreshold;
    [Tooltip("% before interrupting other activities")]
    [Range(0.0f, 1.0f)]
    public float hungerCriticalThreshold;
    [Tooltip("% before interrupting other activities")]
    [Range(0.0f, 1.0f)]
    public float thirstCriticalThresold;
    [Tooltip("how much food has to be remaining for this creature to attempt to eat the food, as % of consumption rate")]
    [Range(0.0f, 1.0f)]
    public float remainingFoodPercent;

    public override void Initialize(GameObject obj)
    {
        Creature creature = obj.GetComponent<Creature>();
        creature.nextTimeForAIUpdate = Time.time;
        creature.logicSM.Initialize(creature);
    }

    public override void ProcessInput(GameObject obj)
    {
        Creature creature = obj.GetComponent<Creature>();

        creature.logicSM.Execute(creature);
    }
}

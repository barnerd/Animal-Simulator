using UnityEngine;

[CreateAssetMenu(fileName = "InputController", menuName = "Input Controller/AI Controller")]
public class AIController : InputController
{
    public float updateInterval = 5f;

    public override void Initialize(GameObject obj)
    {
        obj.GetComponent<Creature>().nextTimeForAIUpdate = Time.time;
    }

    public override void ProcessInput(GameObject obj)
    {
        if (Time.time > obj.GetComponent<Creature>().nextTimeForAIUpdate)
        {
            obj.GetComponent<Creature>().nextTimeForAIUpdate += updateInterval;

            obj.GetComponent<CreatureMotor>().MoveDirection(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)));
        }
    }
}

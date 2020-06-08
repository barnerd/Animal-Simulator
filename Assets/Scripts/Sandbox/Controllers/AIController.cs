using UnityEngine;

[CreateAssetMenu(fileName = "InputController", menuName = "Input Controller/AI Controller")]
public class AIController : InputController
{
    public float updateInterval = 5f;
    float nextTimeForUpdate;

    public override void Initialize(GameObject obj)
    {
        nextTimeForUpdate = Time.time;
    }

    public override void ProcessInput(GameObject obj)
    {
        if (Time.time > nextTimeForUpdate)
        {
            nextTimeForUpdate += updateInterval;
        }
    }
}

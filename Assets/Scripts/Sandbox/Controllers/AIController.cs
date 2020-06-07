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

        // change random direction
        float horizontal = Random.Range(-1f, 1f);
        float vertical = Random.Range(-1f, 1f);

        Vector3 direction = obj.transform.right * horizontal + obj.transform.forward * vertical;
        direction.Normalize();

        obj.GetComponent<CreatureMotor>().Move(direction);
    }
}

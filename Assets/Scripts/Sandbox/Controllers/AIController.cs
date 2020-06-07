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

        Vector3 direction;

        if(obj.GetComponent<Creature>().focus != null)
        {
            if(Vector3.Distance(obj.GetComponent<Creature>().focus.transform.position, obj.transform.position) < obj.GetComponent<Creature>().focus.radius)
            {
                //obj.GetComponent<Creature>().focus = null;
                obj.GetComponent<CreatureMotor>().Move(Vector3.zero);
            }
            else
            {
                direction = (obj.GetComponent<Creature>().focus.transform.position - obj.transform.position).normalized;
                obj.GetComponent<CreatureMotor>().Move(direction);
            }
        }
        else
        {
            // pick a new focus
        }
    }
}

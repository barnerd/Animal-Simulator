using UnityEngine;

public class Billboard : MonoBehaviour
{
    // LateUpdate is called once per frame, after Update
    void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}

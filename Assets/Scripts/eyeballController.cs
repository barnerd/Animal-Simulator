using UnityEngine;

public class eyeballController : MonoBehaviour
{
    private Material eyeMat;

    public Transform target;

    [Header("Pupil")]
    public float pupilSize;
    static public float minPupilSize = 0.254f;
    static public float maxPupilSize = 0.27f;
    static public float minViewingDistance = 2.22f;
    static public float maxViewingDistance = 2.25f;
    private float ratioPupilSizeToViewingDistance;

    // Start is called before the first frame update
    void Start()
    {
        ratioPupilSizeToViewingDistance = (maxPupilSize - minPupilSize) / (maxViewingDistance - minViewingDistance);

        eyeMat = GetComponent<MeshRenderer>().material;

        setPupilSize(minPupilSize);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Limit how far the eyes can look around
        transform.LookAt(target);
    }

    public void SetTarget(Transform _t)
    {
        target = _t;
    }

    public void setPupilSize(float _distance, float _light = 0f)
    {
        float _size = (_distance - minViewingDistance) * ratioPupilSizeToViewingDistance + minPupilSize;

        pupilSize = Mathf.Clamp(_size, minPupilSize, maxPupilSize);

        eyeMat.SetFloat("_PupilRadius", pupilSize);
    }
}

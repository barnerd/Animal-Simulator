using UnityEngine;

public class eyeSetController : MonoBehaviour
{
    [Header("Eyes")]
    public GameObject[] eyesObject;
    private eyeballController[] eyes;
    private Material[] eyeMats;

    [Header("Colors")]
    public Color[] veinColor;
    public Color[] eyeColor;
    public Color[] irisColor;
    public Color[] irisHighlightColor;

    [Header("Parameters")]
    public bool matchingEyes;
    public Transform target;

    private float dist;

    // Start is called before the first frame update
    void Start()
    {
        eyes = new eyeballController[eyesObject.Length];
        eyeMats = new Material[eyesObject.Length];

        for (int i = 0; i < eyesObject.Length; i++)
        {
            eyes[i] = eyesObject[i].GetComponent<eyeballController>();
            eyeMats[i] = eyesObject[i].GetComponent<MeshRenderer>().material;

            if (matchingEyes)
            {
                InitColors(eyeMats[i], eyeColor[0], irisColor[0], irisHighlightColor[0], veinColor[0]);
            }
            else
            {
                InitColors(eyeMats[i], eyeColor[i], irisColor[i], irisHighlightColor[i], veinColor[i]);
            }

            eyes[i].SetTarget(target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(transform.position, target.position);

        foreach (var eye in eyes)
        {
            eye.setPupilSize(dist);
        }
    }

    void InitColors(Material _m, Color _eye, Color _iris, Color _highlight, Color _blood)
    {
        _m.SetColor("_BaseEyeColor", _eye);
        _m.SetColor("_IrisColor", _iris);
        _m.SetColor("_IrisNoiseColor", _highlight);
        _m.SetColor("_BloodVesselsColor", _blood);
    }
}

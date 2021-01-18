using UnityEngine;

public class Sun : HeavenlyBody
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Not needed for the sun
        //base.Update();

        RotateAndRevolve();
    }

    void RotateAndRevolve()
    {
        // TODO: this angle increases/decreases based on heavenlyBody.angleOrbit, eccentricity and equation of ellipse
        Quaternion rot = Quaternion.AngleAxis(orbitCenter.angleRotation * Mathf.Rad2Deg, orbitCenter.axisOfRevolution);
        transform.localPosition = orbitCenter.transform.position + rot * originalDirection;
        // TODO: figure out if Cos is correct
        // TODO: with calendaring, figure out initial angle
        transform.localPosition += new Vector3(0, 0, Mathf.Tan(Mathf.Cos(orbitCenter.angleOrbit) * orbitCenter.angleOfTilt * Mathf.Deg2Rad) * originalDirection.magnitude);
        transform.localRotation = rot;
    }

    public float GetIntensityAtPoint(Vector3 _pos)
    {
        // TODO: Determine Intensity
        // based on time of day and time of year
        // intensity is 1f at noon on the day the planet is closest to the sun
        // intensity is 0f at night
        return 1f;
    }
}


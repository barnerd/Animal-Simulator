using UnityEngine;

public class Moon : HeavenlyBody
{
    // TODO: calcuate phase of the moon based on time of year (sun's position) and moon's orbit (moon's position)
    // path of the moon in the sky is ± the planet's axialTilt + moon's inclination, depending on moon's orbit, and eccentricity and latitude
    // path of the sun in the sky is ± the planet's axialTilt, depending on time of year and eccentricity and latitude, use Analemma
    // calculate moon's size, as a percentage, based on moon's orbit position and eccentricity
    // TODO: calculate shadow's https://www.timeanddate.com/eclipse/shadows.html

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        RotateAndRevolve();
    }

    void RotateAndRevolve()
    {
        // TODO: make the axisOfRevolution rotate. moon's moves 19.4 degress per year

        // TODO: this angle increases/decreases based on heavenlyBody.angleOrbit, eccentricity and equation of ellipse
        Quaternion rot = Quaternion.AngleAxis((orbitCenter.angleRotation) * Mathf.Rad2Deg, orbitCenter.axisOfRevolution);
        transform.localPosition = orbitCenter.transform.position + rot * originalDirection;
        // TODO: figure out if Cos is correct
        // TODO: with calendaring, figure out initial angle
        transform.localPosition += new Vector3(0, 0, Mathf.Tan(Mathf.Cos(orbitCenter.angleRotation) * (orbitCenter.angleOfTilt + inclination) * Mathf.Deg2Rad) * originalDirection.magnitude);
        transform.localRotation = rot;
    }
}

/* variables that change over time are angleRotation and angleOrbit
 * angleRotation is [0, 360) rotation around axialTilt. also, angleRotation / 360 is percent of day
 * angleOrbit is [0, 360) rotation around orbiting body. also, angleOrbit / 360 is percent of year
 * 
 * position in the sky:
 * x-axis - is around the planet - 
 * y-axis - distance from planet - function of angleOrbit
 * z-axis - distance to/from poles - axialTilt as a function of angleOrbit
 * 
 * x+ points towards west
 * y+ points towards sky
 * z+ points towards north
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavenlyBody : MonoBehaviour
{
    public HeavenlyBody orbitCenter;
    public GameObject gfx;

    [Header("Planet Building")]
    public float mass; // expressed as ratio to Sol/Earth
    //[Range(0.05f, 1.25f)]
    public float radius; // expressed as ratio to Sol/Earth
    public float density; // expressed as ratio to Sol/Earth
    public float relativeGravity; // expressed as ratio to Sol/Earth
    public float gravity; // expressed as m/s^2

    [Header("Orbit")]
    public float inclination; // expressed as angle above orbital plane
    public float semiMajorAxis;
    [Range(0f, 0.1f)]
    public float eccentricity; // for the planet, should be on average 0.584 * NumPlanets ^ -1.2
    public float trueAnomoly; // in degrees, the position of the planet around the orbit from the closestDistance
    public float closestDistance;
    public float furthestDistance;
    public float orbitalPeriod;
    public Vector3 axisOfRevolution; // Vector3.back + inclination
    protected Vector3 originalDirection; // direction from heavenlyBody to orbitCenter

    [Header("Seasons")]
    [Range(0f, 45f)]
    public float angleOfTilt;

    [Header("Time")]
    public float angleRotation; // is [0, 2pi) rotation around axialTilt.also, angleRotation / 360 is percent of day
    public float rotationSpeed = 2f * Mathf.PI / (24f * 60f * 60f); // 2pi / 24*60*60
    public float angleOrbit; // is [0, 2pi) rotation around orbiting body.also, angleOrbit / 360 is percent of year
    public float orbitSpeed;
    public GameTime gameTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        relativeGravity = mass / (radius * radius);
        density = relativeGravity / radius;
        gravity = relativeGravity * 9.80665f; // put in terms of m/s^2

        if (orbitCenter != null)
        {
            closestDistance = semiMajorAxis * (1 - eccentricity);
            furthestDistance = semiMajorAxis * (1 + eccentricity);
            orbitalPeriod = Mathf.Pow(semiMajorAxis, 3 / 2);
        }

        axisOfRevolution = Quaternion.Euler(inclination, 0, 0) * Vector3.forward;

        rotationSpeed *= GameTime.GameSecondsPerRealSeconds;
        orbitSpeed = rotationSpeed / 365.24f;
        orbitSpeed *= GameTime.GameSecondsPerRealSeconds;

        if (orbitCenter != null)
            originalDirection = transform.position - orbitCenter.transform.position;

        if (gfx != null)
            gfx.transform.localScale = (radius * 50f) * Vector3.one;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        angleRotation += rotationSpeed * Time.deltaTime;
        angleOrbit += orbitSpeed * Time.deltaTime;
    }
}

/* planets, moons, suns
 * https://youtu.be/x55nxxaWXAM
 * sun:
 * Just make the sun the same
 * MassSun = 1
 * habital zone is 95% to 137% of 1 AU
 * luminosity = L / D^2  = 1 / PlanetOrbitDistance^2, which from below is 1 at semi-major axis
 * 
 * planet:
 * https://youtu.be/RxbIoIM_Uck
 * Mass (relative to earth) = radius ^ 3 * density
 * Mass needs to be within 0.4 to 2.35 * MassEarth
 * radius >200-300 km
 * radius needs to be within 0.78 to 1.25 * RadiusEarth
 * gravity (relative to earth) = mass / radius ^ 2 
 * gravity needs to be within 0.68 to 1.5 * GravityEarth
 * steps: 
 * 1. pick mass & radius
 * 1a. ratio of mass & radius close to 1
 * 2. solve for gravity & density
 * 2a. g = Mass / Radius ^ 2 and density = gravity / radius
 * 
 * https://youtu.be/TrpOJYshfE4
 * eccentricity should be less than 0.2
 * 
 * https://youtu.be/J4K3H9aNLpE
 * tropic region is ±inclination
 * polar region is ±(90-inclination)
 * closest and futhest point on the orbit is semi-major axis * (1±eccentricity)
 * orbit period (semi-major axis^3 / MassSun)^1/2 or semi-major axes^3/2
 * steps:
 * 1. pick inclination
 * 2. pick semi-major axis
 * 3. pick eccentricity
 * 4. solve for orbit period
 * 
 * moons:
 * Mass (relative to earth) = radius ^ 3 * density
 * radius >200-300 km or .05 * RadiusEarth
 * gravity (relative to earth) = mass / radius ^ 2 
 * 
 * moon orbit:
 * https://youtu.be/t6i6TPsqvaM
 * Outer Radius = a * (MassPlanet / MassSun)^1/3 * 235 = a * MassPlanet ^ 1/3 * 235
 * Inner Radius = 2.44 * Radiusplanet * (densityearth * densitymoon)^1/3
 * should be less than half the Outer Radius
 * 
 * should be prograde orbit, goes in the same direction as the planet
 * should have low inclination (<5 degrees)
 * should have low eccentricity (~0)
 * 
 * multiple moons need to be 10 * radiusplanet apart 
 * Period = 0.0588 * (radiusOrbit^3 / (MassPlanet + MassMoon))^1/2
 * 
 * https://youtu.be/1sM6YBlKgg4?t=252
 * size of the sun/moon in the sky is diameterBody / DistanceBody
 * earthSun and earthMoon are 0.5 degrees
 */

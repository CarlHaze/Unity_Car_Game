using Unity.VisualScripting;
using UnityEngine;

public class SuspensionRayCast : MonoBehaviour
{
    public Transform tireTransform;
    private Rigidbody carRigidBody;

    public float springStrength = 100f;
    public float springDamper = 10f;
    public float suspensionRestDist = 0.5f; //Distance where the suspension is at rest 
    public float maxSuspensionDist = 1.0f; // Maximum suspension travel distance

    private RaycastHit tireRay;
    private bool rayDidHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Cast a ray downwards from the tire's position to check for ground
        rayDidHit = Physics.Raycast(tireTransform.position, -tireTransform.up, out tireRay, maxSuspensionDist);

        // Suspension calculation only if the ray hit something
        if (rayDidHit)
        {
            // world-space direction of the spring force.
            Vector3 springDir = tireTransform.up;

            // world-space velocity of this tire
            Vector3 tireWorldVel = carRigidBody.GetPointVelocity(tireTransform.position);

            // calculate offset from the raycast
            float offset = suspensionRestDist - tireRay.distance;

            //calculate velocity along the spring direction
            // note that springDir is a unity vector, so this returns the magnitude of tireWorldVel
            // as projected into springDir
            float vel = Vector3.Dot(springDir, tireWorldVel);

            //calculate the magnitude of the dampend spring force
            float force = (offset * springStrength) - (vel * springDamper);

            //apply the foece at the location of this tire, in the direction of the suspension
            carRigidBody.AddForceAtPosition(springDir * force, tireTransform.position);
        };

    }





}

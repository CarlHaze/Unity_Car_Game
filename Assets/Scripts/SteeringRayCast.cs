//using UnityEngine;

//public class SteeringRayCast : MonoBehaviour
//{

//    public Transform tireTransform;
//    private Rigidbody carRigidBody;


//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //steering force
//        if (rayDidHit)
//        {
//            //world-space direction of the spring force.
//            Vector3 steeringDir = tireTransform.right;

//            // world-space velocity of the suspension
//            Vector3 tireWorldVel = carRigidBody.GetPointVelocity(tireTransform.position);

//            // what is the tire's velocity in the steering direction?
//            //note that steeringDir is a unit vector, so this returns the magnitude of the tireWorldVel as projected onto steeringDir
//            float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);

//            //the change in velocity that we are looking for is -steeringVel * gridFactor
//            //gripFactor is in a range of 0-1, 0 means no grip, 1 means full grip
//            float desiredVelChange = -steeringVel * tireGripFactor;

//            // turn change in velocity into an acceleration (acceleration = change in vel / time)
//            // this will produce the acceleration necessary to change the velocity by desiredVelCHange in 1 physics step
//            float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

//            // Force = Mass * Acceleration, so mutiply by the mass of the tire and apply as a force
//            carRigidBody.AddForceAtPosition(steeringDir * tireMass * desiredAccel, tireTransform.position);

//        }
//    }

  
//}

//using UnityEngine;

//public class accelerationRayCast : MonoBehaviour
//{
//    public Transform carTransform;
//    public Transform tireTransform;
//    private Rigidbody carRigidBody;
//    public float carTopSpeed = 10;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (rayDidHit)
//        {
//            // world-space direction of the acceleration/braking force.
//            Vector3 accelDir = tireTransform.forward;

//            if (accelInput > 0.0f)
//            {
//                // forward speed of the car (in the direction of driving)
//                float carSpeed = Vector3.Dot(carTransform.forward, carRigidBody.linearVelocity);

//                //  normalized car speed
//                float normalizedSpeed = Mathf.Clamp(Mathf.Abs(carSpeed) / carTopSpeed);

//                // avaliable torque
//                float avaliableTorque = powerCurve.Evaluate(normalizedSpeed) * accelInput;

//                carRigidBody.AddForceAtPosition(accelDir * avaliableTorque, tireTransform.position);
//            }
//        }
        
//    }
//}

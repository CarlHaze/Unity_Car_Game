using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class CarControl : MonoBehaviour
{
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public Vector3 centerOfMass; // Expose center of mass to allow it to be set from the inspector

    private WheelControl[] wheels;
    private Rigidbody rigidBody;

    // Input actions for accelerate, brake, and steer
    InputAction accelerateAction;
    InputAction brakeAction;
    InputAction steerAction;

    public Light LeftBrakeLight;
    public Light RightBrakeLight;

    // Gearing and RPM system
    public float[] gearRatios = { 3.0f, 2.0f, 1.5f, 1.0f, 0.8f }; // Example gear ratios
    public float finalDriveRatio = 3.42f;
    public float maxRPM = 7000;
    public float idleRPM = 800;
    private int currentGear = 0;
    private float currentRPM = 0;

    private void Awake()
    {
        // Initialize input actions here to ensure they're set before OnEnable
        accelerateAction = InputSystem.actions.FindAction("Accelerate");
        brakeAction = InputSystem.actions.FindAction("Brake");
        steerAction = InputSystem.actions.FindAction("Move");
    }

    private void OnEnable()
    {
        // Enable input actions
        accelerateAction.Enable();
        brakeAction.Enable();
        steerAction.Enable();
    }

    private void OnDisable()
    {
        // Disable input actions
        accelerateAction.Disable();
        brakeAction.Disable();
        steerAction.Disable();
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Set the center of mass from the inspector
        rigidBody.centerOfMass = centerOfMass;

        // Find all child GameObjects that have the WheelControl script attached
        wheels = GetComponentsInChildren<WheelControl>();

        // Ensure brake lights are off at the start
        LeftBrakeLight.enabled = false;
        RightBrakeLight.enabled = false;
    }

    public float GetCurrentSpeed()
    {
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);
        return Mathf.Abs(forwardSpeed * 3.6f); // Convert to km/h for display
    }

    public float GetCurrentRPM()
    {
        return currentRPM;
    }

    public int GetCurrentGear()
    {
        return currentGear + 1; // +1 to make it 1-based instead of 0-based
    }

    void FixedUpdate()
    {
        // Combine keyboard (WASD or arrow keys) and controller input for acceleration and braking
        float vInput = (accelerateAction.ReadValue<float>() - brakeAction.ReadValue<float>()) + Input.GetAxis("Vertical");

        // Read steering from both keyboard and controller input
        float hInput = steerAction.ReadValue<Vector2>().x + Input.GetAxis("Horizontal");

        // Calculate current speed in relation to the forward direction of the car
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);
        float currentSpeed = Mathf.Abs(forwardSpeed * 3.6f); // Convert to km/h for display

        // Calculate how close the car is to top speed
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Calculate available torque and steering range
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check if the car is accelerating in the same direction as its velocity
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);
        bool isBraking = brakeAction.ReadValue<float>() > 0; // Check if the brake is applied

        // Control the brake lights
        if (isBraking)
        {
            LeftBrakeLight.enabled = true;
            RightBrakeLight.enabled = true;
        }
        else
        {
            LeftBrakeLight.enabled = false;
            RightBrakeLight.enabled = false;
        }

        // Calculate RPM based on current speed and gear ratio
        currentRPM = Mathf.Clamp((currentSpeed / gearRatios[currentGear]) * finalDriveRatio * 60, idleRPM, maxRPM);

        // Calculate torque and horsepower
        float torque = currentMotorTorque * (currentRPM / maxRPM);
        float horsepower = (torque * currentRPM) / 5252;

        // Automatic transmission logic
        if (currentRPM > maxRPM * 0.9f && currentGear < gearRatios.Length - 1)
        {
            currentGear++;
        }
        else if (currentRPM < maxRPM * 0.3f && currentGear > 0)
        {
            currentGear--;
        }

        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            if (isAccelerating)
            {
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * torque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }
}

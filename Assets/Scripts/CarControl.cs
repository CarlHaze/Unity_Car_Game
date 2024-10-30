using UnityEngine.InputSystem;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    public float motorTorque = 2500;
    public float brakeTorque = 2700;
    public float maxSpeed = 200;  // Increased max speed for RX-7
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public Vector3 centerOfMass;

    private WheelControl[] wheels;
    private Rigidbody rigidBody;

    // Input actions for acceleration and steering
    InputAction accelerateAction;
    InputAction brakeAction;
    InputAction steerAction;

    public Light LeftBrakeLight;
    public Light RightBrakeLight;

    // Gearing and RPM system
    public float[] gearRatios = { 3.475f, 2.002f, 1.366f, 1.0f, 0.756f };
    public float finalDriveRatio = 4.1f; // RX-7 FC final drive ratio
    public float maxRPM = 7000f; // RX-7 FC max RPM

    [SerializeField] private int currentGear = 0;
    [SerializeField] private float currentRPM = 0;

    private void Awake()
    {
        accelerateAction = InputSystem.actions.FindAction("Accelerate");
        brakeAction = InputSystem.actions.FindAction("Brake");
        steerAction = InputSystem.actions.FindAction("Move");
    }

    private void OnEnable()
    {
        accelerateAction.Enable();
        brakeAction.Enable();
        steerAction.Enable();
    }

    private void OnDisable()
    {
        accelerateAction.Disable();
        brakeAction.Disable();
        steerAction.Disable();
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = centerOfMass;
        wheels = GetComponentsInChildren<WheelControl>();
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
        return currentGear + 1; // +1 to make it 1-based
    }

    void FixedUpdate()
    {
        float vInput = (accelerateAction.ReadValue<float>() - brakeAction.ReadValue<float>()) + Input.GetAxis("Vertical");
        float hInput = steerAction.ReadValue<Vector2>().x + Input.GetAxis("Horizontal");

        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);
        float currentSpeed = Mathf.Abs(forwardSpeed * 3.6f); // km/h
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        bool isBraking = brakeAction.ReadValue<float>() > 0;
        LeftBrakeLight.enabled = RightBrakeLight.enabled = isBraking;

        if (vInput > 0)
        {
            // Calculate current RPM based on speed, gear ratio, and throttle
            currentRPM = Mathf.Lerp(
                currentRPM,
                Mathf.Clamp((currentSpeed / gearRatios[currentGear]) * finalDriveRatio * 60, 0, maxRPM),
                Time.deltaTime * 5f
            );
        }
        else
        {
            // Smoothly decrease RPM to 0 when not accelerating
            currentRPM = Mathf.Lerp(currentRPM, 0, Time.deltaTime * 2f);
        }

        // Automatic transmission shifting logic
        if (currentRPM > maxRPM * 0.9f && currentGear < gearRatios.Length - 1)
        {
            currentGear++;
            currentRPM = Mathf.Clamp(currentRPM / 2, 0, maxRPM);
        }
        else if (currentRPM < maxRPM * 0.3f && currentGear > 0)
        {
            currentGear--;
            currentRPM = Mathf.Clamp(currentRPM * 1.5f, 0, maxRPM);
        }

        // Apply torque to motorized wheels only when accelerating
        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            if (vInput > 0)
            {
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                wheel.WheelCollider.brakeTorque = brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class RX7CarController : MonoBehaviour
{
    // Car specifications
    public float enginePower = 147f; // HP
    public float maxRPM = 6500f;
    public float[] gearRatios = { 3.475f, 2.002f, 1.366f, 1.00f, 0.756f };
    public float finalDriveRatio = 4.10f;
    public float curbWeight = 1223f; // kg
    public Vector3 centerOfMass;

    // Steering specs
    public float MaxAngleAtSpeed = 10;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;

    // Wheel colliders
    private WheelControl[] wheels;

    // Rigidbody
    private Rigidbody rb;

    // Current state
    private int currentGear = 0;
    private float currentRPM = 0f;

    // Input actions for acceleration and steering (from Input System)
    InputAction accelerateAction;
    InputAction brakeAction;
    InputAction steerAction;
    InputAction moveAction;

    private void Awake()
    {
        var inputActions = new InputActionMap("CarControls");
        accelerateAction = inputActions.AddAction("Accelerate", binding: "<Keyboard>/w");
        brakeAction = inputActions.AddAction("Brake", binding: "<Keyboard>/s");
        moveAction = inputActions.AddAction("Move", binding: "<Keyboard>/arrow");
        steerAction = inputActions.AddAction("Steer", binding: "<Gamepad>/leftStick");

        inputActions.Enable();
    }

    private void OnEnable()
    {
        accelerateAction.Enable();
        brakeAction.Enable();
        moveAction.Enable();
        steerAction.Enable();
    }

    private void OnDisable()
    {
        accelerateAction.Disable();
        brakeAction.Disable();
        moveAction.Disable();
        steerAction.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
        rb.mass = curbWeight;
        wheels = GetComponentsInChildren<WheelControl>();
    }

    void Update()
    {
        HandleInput();
        UpdateEngineRPM();
    }

    void HandleInput()
    {
        // Simple gear shifting logic
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentGear < gearRatios.Length - 1)
        {
            currentGear++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && currentGear > 0)
        {
            currentGear--;
        }
    }

    private void FixedUpdate()
    {
        float vInput = accelerateAction.ReadValue<float>() - brakeAction.ReadValue<float>();
        float hInput = moveAction.ReadValue<Vector2>().x + steerAction.ReadValue<Vector2>().x;

        float currentSpeed = rb.linearVelocity.magnitude * 3.6f; // Convert to km/h
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, currentSpeed / MaxAngleAtSpeed);
        float currentMotorTorque = enginePower * 5252f / currentRPM;

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
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * currentMotorTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }

    void UpdateEngineRPM()
    {
        // Calculate engine RPM based on wheel RPM and gear ratios
        float wheelRPM = 0f;
        int motorizedWheels = 0;

        foreach (var wheel in wheels)
        {
            if (wheel.motorized)
            {
                wheelRPM += wheel.WheelCollider.rpm;
                motorizedWheels++;
            }
        }

        if (motorizedWheels > 0)
        {
            wheelRPM /= motorizedWheels;
        }

        currentRPM = wheelRPM * finalDriveRatio * gearRatios[currentGear];
        currentRPM = Mathf.Clamp(currentRPM, 0, maxRPM);
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaceManager : MonoBehaviour
{
    public Transform[] respawnPoints; // Array of respawn points in your scene
    public Transform startPos; // Reference to the StartPos GameObject
    public GameObject carPrefab; // Reference to the car prefab

    private GameObject car; // Reference to the instantiated car GameObject
    private Transform nearestRespawnPoint;
    private InputAction reset;
    private LapTimer lapTimer; // Reference to the LapTimer component

    // Speed UI
    public TextMeshProUGUI speedText;
    private CarControl carControl;

    private void Awake()
    {
        // Initialize input actions here to ensure they're set before OnEnable
        reset = InputSystem.actions.FindAction("Select");
        lapTimer = FindFirstObjectByType<LapTimer>(); // Find the LapTimer in the scene

        // Instantiate the car prefab at the start position
        if (carPrefab != null && startPos != null)
        {
            car = Instantiate(carPrefab, startPos.position, startPos.rotation);

            // Get the CarControl component from the instantiated car
            carControl = car.GetComponent<CarControl>();

            // Find the main camera and get its FollowCamera component
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                FollowCamera followCamera = mainCamera.GetComponent<FollowCamera>();
                if (followCamera != null)
                {
                    followCamera.target = car.transform;
                }
                else
                {
                    Debug.LogError("Main camera does not have a FollowCamera component.");
                }
            }
            else
            {
                Debug.LogError("Main camera not found.");
            }
        }
        else
        {
            Debug.LogError("Car prefab or start position is not set.");
        }
    }

    private void OnEnable()
    {
        // Enable input actions
        reset.Enable();
    }

    private void OnDisable()
    {
        // Disable input actions when not needed
        reset.Disable();
    }

    void Update()
    {
        // Check if the player presses the respawn button
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCarPosition();
        }

        // Check if the Select action is triggered
        if (reset.triggered)
        {
            ResetCarPosition();
        }

        // Update the speed text with the current speed from CarControl
        if (carControl != null && speedText != null)
        {
            int currentSpeed = Mathf.RoundToInt(carControl.GetCurrentSpeed());
            speedText.text = "Speed: " + currentSpeed.ToString();
        }
    }

    void ResetCarPosition()
    {
        if (car == null)
        {
            Debug.LogError("Car reference is not set.");
            return;
        }

        // Check if the lap timer has started
        if (lapTimer != null && !lapTimer.isTiming)
        {
            // If the timer has not started, reset to StartPos
            car.transform.position = startPos.position;
            car.transform.rotation = startPos.rotation; // Align rotation if needed

            // Reset any unwanted velocity
            Rigidbody carRigidbody = car.GetComponent<Rigidbody>();
            carRigidbody.linearVelocity = Vector3.zero;
            carRigidbody.angularVelocity = Vector3.zero;

            Debug.Log("Car reset to StartPos.");
            return;
        }

        // If the lap timer has started, reset to the nearest respawn point
        nearestRespawnPoint = FindNearestRespawn();

        if (nearestRespawnPoint != null)
        {
            // Set the car's position to match the respawn point
            car.transform.position = nearestRespawnPoint.position;

            // Rotate the car to align with the X-axis of the respawn point
            Vector3 respawnForward = nearestRespawnPoint.right; // X-axis of respawn point
            car.transform.rotation = Quaternion.LookRotation(respawnForward);

            // Reset any unwanted velocity
            Rigidbody carRigidbody = car.GetComponent<Rigidbody>();
            carRigidbody.linearVelocity = Vector3.zero;
            carRigidbody.angularVelocity = Vector3.zero;

            Debug.Log("Car reset to nearest respawn point.");
        }
    }

    Transform FindNearestRespawn()
    {
        Transform closestPoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform point in respawnPoints)
        {
            Vector3 toRespawn = point.position - car.transform.position;
            float distance = toRespawn.sqrMagnitude;

            // Check all respawn points without the direction condition
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }
}
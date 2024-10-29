using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnController : MonoBehaviour
{
    public Transform[] respawnPoints; // Array of respawn points in your scene
    private Transform nearestRespawnPoint;
    public Transform startPos; // Reference to the StartPos GameObject

    InputAction reset;
    private LapTimer lapTimer; // Reference to the LapTimer component

    private void Awake()
    {
        // Initialize input actions here to ensure they're set before OnEnable
        reset = InputSystem.actions.FindAction("Select");
        lapTimer = FindFirstObjectByType<LapTimer>(); // Find the LapTimer in the scene
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
    }

    void ResetCarPosition()
    {
        // Check if the lap timer has started
        if (lapTimer != null && !lapTimer.isTiming)
        {
            // If the timer has not started, reset to StartPos
            transform.position = startPos.position;
            transform.rotation = startPos.rotation; // Align rotation if needed

            // Reset any unwanted velocity
            Rigidbody carRigidbody = GetComponent<Rigidbody>();
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
            transform.position = nearestRespawnPoint.position;

            // Rotate the car to align with the X-axis of the respawn point
            Vector3 respawnForward = nearestRespawnPoint.right; // X-axis of respawn point
            transform.rotation = Quaternion.LookRotation(respawnForward);

            // Reset any unwanted velocity
            Rigidbody carRigidbody = GetComponent<Rigidbody>();
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
            Vector3 toRespawn = point.position - transform.position;
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

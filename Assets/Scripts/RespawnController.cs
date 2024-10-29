using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnController : MonoBehaviour
{
    public Transform[] respawnPoints; // Array of respawn points in your scene
    private Transform nearestRespawnPoint;

    InputAction reset;

    private void Awake()
    {
        // Initialize input actions here to ensure they're set before OnEnable
        reset = InputSystem.actions.FindAction("Select");
    }

    private void OnEnable()
    {
        // Enable input actions
        reset.Enable();
    
    }

    void Update()
    {
        // Check if the player presses the respawn button
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetToNearestRespawn();
        }

        if (reset.triggered)
        {
            ResetToNearestRespawn();
        }
        
    }

    void ResetToNearestRespawn()
    {
        // Find the closest respawn point behind the car
        nearestRespawnPoint = FindNearestRespawnBehind();

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
        }
    }

    Transform FindNearestRespawnBehind()
    {
        Transform closestPoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform point in respawnPoints)
        {
            Vector3 toRespawn = point.position - transform.position;
            float distance = toRespawn.sqrMagnitude;

            // Only consider points behind the car
            if (Vector3.Dot(transform.forward, toRespawn) < 0 && distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }


}

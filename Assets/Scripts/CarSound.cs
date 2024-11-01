using UnityEngine;

public class CarSound : MonoBehaviour
{
    public AudioSource idleSound; // Reference to the idle sound AudioSource
    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>(); // Get the Rigidbody component
        if (idleSound != null)
        {
            idleSound.Stop(); // Ensure idle sound is stopped at start
        }
    }

    private void Update()
    {
        // Check if the car is nearly stationary
        float carSpeed = rigidBody.linearVelocity.magnitude; // Get speed as a scalar value

        if (carSpeed < 0.1f) // If the car is close to stationary
        {
            if (!idleSound.isPlaying)
            {
                idleSound.Play(); // Play idle sound
            }
        }
        else // If the car is moving
        {
            if (idleSound.isPlaying)
            {
                idleSound.Stop(); // Stop idle sound
            }
        }
    }
}

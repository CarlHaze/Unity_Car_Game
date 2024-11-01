using UnityEngine;

public class CarAudioManager : MonoBehaviour
{
    public AudioClip idleSound;      // Reference to the idle sound AudioClip
    public AudioClip lowRangeClip;   // Low-range driving sound

    private AudioSource idleAudioSource;
    private AudioSource drivingAudioSource;

    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>(); // Get the Rigidbody component

        // Create and configure the idle AudioSource
        idleAudioSource = gameObject.AddComponent<AudioSource>();
        idleAudioSource.clip = idleSound;
        idleAudioSource.loop = true;

        // Create and configure the driving AudioSource
        drivingAudioSource = gameObject.AddComponent<AudioSource>();
        drivingAudioSource.clip = lowRangeClip; // Set low range as the driving sound
        drivingAudioSource.loop = true;
    }

    private void Update()
    {
        float carSpeed = GetCurrentSpeed(); // Get current speed from your CarControl script

        if (carSpeed < 0.1f) // If the car is nearly stationary
        {
            PlayIdleSound();
            StopDrivingSound();
        }
        else // If the car is moving
        {
            StopIdleSound();
            PlayDrivingSound(carSpeed);
        }
    }

    private void PlayIdleSound()
    {
        if (!idleAudioSource.isPlaying)
        {
            idleAudioSource.Play(); // Play idle sound
        }
    }

    private void StopIdleSound()
    {
        if (idleAudioSource.isPlaying)
        {
            idleAudioSource.Stop(); // Stop idle sound
        }
    }

    private void PlayDrivingSound(float speed)
    {
        if (!drivingAudioSource.isPlaying)
        {
            drivingAudioSource.Play(); // Start playing the driving sound
        }

        // Adjust the pitch based on speed
        drivingAudioSource.pitch = Mathf.Clamp(speed / 50f, 0.5f, 2f); // Scale pitch based on speed
        drivingAudioSource.volume = Mathf.Clamp(speed / 100f, 0.1f, 1f); // Scale volume based on speed
    }

    private void StopDrivingSound()
    {
        if (drivingAudioSource.isPlaying)
        {
            drivingAudioSource.Stop(); // Stop driving sound
        }
    }

    private float GetCurrentSpeed()
    {
        return rigidBody.linearVelocity.magnitude * 3.6f; // Convert to km/h for display
    }
}

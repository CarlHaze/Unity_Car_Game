using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public LapTimer lapTimer; // Reference to the LapTimer script

    private void OnTriggerEnter(Collider other)
    {
        // Check if the car has entered the checkpoint
        if (other.CompareTag("Car"))
        {
            lapTimer.CheckpointPassed(gameObject); // Notify LapTimer of the checkpoint pass
            Debug.Log("Checkpoint passed: " + gameObject.name);
        }
    }
}

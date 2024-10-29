using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LapTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bestLapText;

    public GameObject[] checkpoints; // Array for checkpoint objects
    private HashSet<GameObject> checkpointsPassed = new HashSet<GameObject>(); // Track passed checkpoints

    private float elapsedTime = 0f;
    private bool isTiming = false;
    private float bestLapTime = float.MaxValue;

    private void Update()
    {
        // Update the timer only if it's active
        if (isTiming)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is the car
        if (other.CompareTag("Car"))
        {
            if (!isTiming)
            {
                // Start timing when crossing the start line if not already started
                isTiming = true;
                elapsedTime = 0f;
                checkpointsPassed.Clear(); // Reset checkpoints for the new lap
                Debug.Log("Lap started. Timer reset.");
            }
            else if (AllCheckpointsPassed())
            {
                // If all checkpoints were passed, complete the lap
                Debug.Log("Lap completed!");
                if (elapsedTime < bestLapTime)
                {
                    bestLapTime = elapsedTime;
                    UpdateBestLapDisplay();
                    Debug.Log("New best lap time recorded: " + bestLapText.text);
                }

                elapsedTime = 0f; // Reset the timer for the next lap
                checkpointsPassed.Clear(); // Reset checkpoints for the new lap
                Debug.Log("Timer reset for new lap.");
            }
            else
            {
                Debug.Log("Lap incomplete. Not all checkpoints have been passed.");
            }
        }
    }

    public void CheckpointPassed(GameObject checkpoint)
    {
        // Add checkpoint to passed list if it’s in the array and hasn't been counted yet
        if (System.Array.Exists(checkpoints, cp => cp == checkpoint))
        {
            checkpointsPassed.Add(checkpoint);
            Debug.Log("Checkpoint added: " + checkpoint.name);
        }
    }

    private bool AllCheckpointsPassed()
    {
        // Check if all checkpoints have been passed
        bool allPassed = checkpointsPassed.Count == checkpoints.Length;
        Debug.Log("All checkpoints passed: " + allPassed);
        return allPassed;
    }

    private void UpdateTimerDisplay()
    {
        // Format the elapsed time as minutes:seconds:milliseconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);

        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }

    private void UpdateBestLapDisplay()
    {
        // Format the best lap time as minutes:seconds:milliseconds
        int minutes = Mathf.FloorToInt(bestLapTime / 60f);
        int seconds = Mathf.FloorToInt(bestLapTime % 60f);
        int milliseconds = Mathf.FloorToInt((bestLapTime * 1000) % 1000);

        bestLapText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}

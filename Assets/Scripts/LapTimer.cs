using UnityEngine;
using TMPro;

public class LapTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; 
    public TextMeshProUGUI bestLapText; 
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
        // Start the timer or record a finish time when the car enters the trigger zone
        if (other.CompareTag("Car"))
        {
            if (!isTiming)
            {
             
                isTiming = true;
                elapsedTime = 0f;
            }
            else
            {
                // Stop the timer on the next pass (finish line)
                // Check if the current lap time is better than the best lap time
                if (elapsedTime < bestLapTime)
                {
                    // Update the best lap time
                    bestLapTime = elapsedTime;
                    // Update the best lap display
                    UpdateBestLapDisplay();
                    elapsedTime = 0f; // Reset elapsed time here after checking the lap
                }

                // Reset the timer for the next lap immediately after recording the lap
                elapsedTime = 0f; // Reset elapsed time here after checking the lap
               
            }
        }
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

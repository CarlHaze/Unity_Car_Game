using UnityEngine;

public class DropOffStation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the car entered the drop-off station
        if (other.CompareTag("Car"))
        {
            // Call the DropOff method on CollectableManager
            CollectableManager.Instance.DropOff();
        }
    }
}

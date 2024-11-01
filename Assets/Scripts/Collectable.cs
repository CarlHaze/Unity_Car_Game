using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with the collectable is the Car
        if (other.CompareTag("Car"))
        {
            // Call the Collect method on the CollectableManager and pass in this collectable
            CollectableManager.Instance.Collect(gameObject);
        }
    }
}

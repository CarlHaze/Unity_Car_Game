using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    public static CollectableManager Instance;
    public int collectableCount = 0;   // Number of currently collected items
    public int deliveryCount = 0;      // Total number of items delivered

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Collect(GameObject collectable)
    {
        Destroy(collectable);
        collectableCount++;
        Debug.Log("Collectable Count: " + collectableCount);
    }

    public void DropOff()
    {
        if (collectableCount > 0)
        {
            // Add all collected items to delivery count
            deliveryCount += collectableCount;
            Debug.Log("Delivery Count: " + deliveryCount);

            // Reset collectable count after drop-off
            collectableCount = 0;
        }
        else
        {
            Debug.Log("No collectables to drop off!");
        }
    }
}

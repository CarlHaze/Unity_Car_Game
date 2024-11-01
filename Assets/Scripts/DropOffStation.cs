using UnityEngine;

public class DropOffStation : MonoBehaviour
{
    /*
     verify if the player has arrived at the correct address and deliver the package if it matches the current delivery target.
     */
    public string address;  // The specific address of this drop-off station

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            foreach (var package in PackageManager.Instance.packages)
            {
                if (package.isCollected && !package.isDelivered)
                {
                    PackageManager.Instance.DeliverPackage(package, address);
                }
            }
        }
    }
}

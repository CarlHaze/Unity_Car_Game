using UnityEngine;

public class Depot : MonoBehaviour
{

    /*
     manage interactions with the depot where the player can collect packages. This script could, for instance, spawn new packages and add them to the PackageManager.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            foreach (var package in PackageManager.Instance.packages)
            {
                if (!package.isCollected)
                {
                    PackageManager.Instance.CollectPackage(package);
                }
            }
        }
    }
}

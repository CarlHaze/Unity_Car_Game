using System.Collections.Generic;
using UnityEngine;

public class PackageManager : MonoBehaviour
{

    /*
    keep track of the packages, assign them addresses, and handle package collection and delivery logic.
    */
    public static PackageManager Instance;

    public List<Package> packages = new List<Package>();
    public int deliveryCount = 0;

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

    // Method to add a new package with a given address
    public void AddPackage(string address)
    {
        packages.Add(new Package(address));
    }

    // Method for collecting packages at the depot
    public void CollectPackage(Package package)
    {
        package.isCollected = true;
        Debug.Log("Collected package for: " + package.address);
    }

    // Method for delivering packages to the correct address
    public void DeliverPackage(Package package, string deliveryAddress)
    {
        if (package.isCollected && package.address == deliveryAddress && !package.isDelivered)
        {
            package.isDelivered = true;
            deliveryCount++;
            Debug.Log("Delivered package to: " + deliveryAddress);
        }
        else if (!package.isCollected)
        {
            Debug.Log("Package not collected yet!");
        }
        else if (package.isDelivered)
        {
            Debug.Log("Package already delivered!");
        }
        else
        {
            Debug.Log("Wrong delivery address!");
        }
    }
}

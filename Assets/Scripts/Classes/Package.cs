using UnityEngine;

[System.Serializable] // This attribute makes the class serializable for use in the Inspector
public class Package
{
    public string address; // The address where the package should be delivered
    public bool isCollected = false; // Indicates if the package has been collected
    public bool isDelivered = false; // Indicates if the package has been delivered

    // Constructor to initialize the package with an address
    public Package(string address)
    {
        this.address = address;
    }
}

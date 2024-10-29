//using UnityEngine;

//public class RaceManager : MonoBehaviour
//{
//    public GameObject carPrefab; // Drag the car prefab here
//    public Transform startPosition; // Drag the start position GameObject here

//    private GameObject carInstance;
//    private RespawnController respawnController; // Reference to the RespawnController

//    void Start()
//    {
//        respawnController = GetComponent<RespawnController>(); // Get the RespawnController attached to the same GameObject
//        SpawnCar();

//    }

//    void SpawnCar()
//    {
//        if (carPrefab != null && startPosition != null)
//        {
//            // Instantiate the car prefab at the start position
//            carInstance = Instantiate(carPrefab, startPosition.position, startPosition.rotation);

//            // Reset any unwanted velocity
//            Rigidbody carRigidbody = carInstance.GetComponent<Rigidbody>();
//            if (carRigidbody != null)
//            {
//                carRigidbody.linearVelocity = Vector3.zero;
//                carRigidbody.angularVelocity = Vector3.zero;
//            }

//            // Set the car transform in the RespawnController
//            respawnController.SetCarTransform(carInstance.transform);
//        }
//        else
//        {
//            Debug.LogWarning("CarPrefab or StartPosition not assigned in RaceManager.");
//        }
//    }
//}

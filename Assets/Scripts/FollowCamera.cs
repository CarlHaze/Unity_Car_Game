using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;                   // Reference to the car
    public Vector3 offset = new Vector3(0, 5, -10); // Offset behind the car
    public float followSpeed = 10f;            // Speed of the camera movement
    public float rotationSpeed = 5f;           // Speed of the camera rotation

    void FixedUpdate()
    {
        if (target == null) return;

        // Target position with the offset
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        // Smoothly move camera position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.fixedDeltaTime);

        // Smoothly rotate the camera to look at the target
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }
}

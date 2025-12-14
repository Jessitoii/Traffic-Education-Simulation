using UnityEngine;

public class VRThirdPersonCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -6);
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate desired position
        // TransformPoint converts local offset to world space, handling rotation
        Vector3 desiredPosition = target.TransformPoint(offset);
        
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Look at the car
        transform.LookAt(target);
    }
}

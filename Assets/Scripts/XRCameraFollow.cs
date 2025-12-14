using UnityEngine;

public class XRCameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2.5f, -5f);
    public float smoothSpeed = 0.125f;
    public float rotationSmoothness = 5f; // Increased for snappier rotation

    private bool _hasSnapped = false;

    void Start()
    {
        // Try to find target if missing
        if (target == null)
        {
            GameObject car = GameObject.Find("PlayerCar");
            if (car != null) target = car.transform;
        }

        SnapToTarget();
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (!_hasSnapped)
        {
            SnapToTarget();
        }

        // Position
        Vector3 desiredPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Rotation
        // We want the camera rig to face the same way as the car (yaw only)
        float targetYaw = target.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, targetYaw, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothness);
    }

    public void SnapToTarget()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.TransformPoint(offset);
            transform.position = desiredPosition;
            
            float targetYaw = target.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, targetYaw, 0);
            
            _hasSnapped = true;
        }
    }
}

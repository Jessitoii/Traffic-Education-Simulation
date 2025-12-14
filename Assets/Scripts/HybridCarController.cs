using UnityEngine;
using UnityEngine.InputSystem;

public class HybridCarController : MonoBehaviour
{
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    public float motorForce = 1500f;
    public float maxSteerAngle = 30f;
    
    public InputActionProperty xrMoveInput;

    private void OnEnable()
    {
        if (xrMoveInput.action != null) xrMoveInput.action.Enable();
    }

    private void OnDisable()
    {
        if (xrMoveInput.action != null) xrMoveInput.action.Disable();
    }

    void FixedUpdate()
    {
        float v = 0;
        float h = 0;

        // Keyboard Input (Legacy or New Input System via Keyboard class if needed, but user asked for Input.GetAxis)
        // We try-catch just in case Input.GetAxis throws (if legacy is disabled)
        try
        {
            v = Input.GetAxis("Vertical");
            h = Input.GetAxis("Horizontal");
        }
        catch (System.Exception)
        {
            // Fallback if Legacy Input is disabled
            if (Keyboard.current != null)
            {
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v = 1;
                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v = -1;
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h = -1;
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h = 1;
            }
        }

        // XR Input Override
        if (xrMoveInput.action != null)
        {
            Vector2 xrVal = xrMoveInput.action.ReadValue<Vector2>();
            if (xrVal.sqrMagnitude > 0.1f)
            {
                h = xrVal.x;
                v = xrVal.y;
            }
        }

        float steer = h * maxSteerAngle;
        float motor = v * motorForce;

        if (frontLeft) { frontLeft.steerAngle = steer; frontLeft.motorTorque = motor; }
        if (frontRight) { frontRight.steerAngle = steer; frontRight.motorTorque = motor; }
        if (rearLeft) { rearLeft.motorTorque = motor; }
        if (rearRight) { rearRight.motorTorque = motor; }
    }
}

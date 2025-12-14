using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleXRCarController : MonoBehaviour
{
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    public float motorForce = 1500f;
    public float maxSteerAngle = 30f;

    public InputActionProperty moveInput;

    private void OnEnable()
    {
        if (moveInput.action != null)
            moveInput.action.Enable();
    }

    private void OnDisable()
    {
        if (moveInput.action != null)
            moveInput.action.Disable();
    }

    void FixedUpdate()
    {
        Vector2 input = Vector2.zero;
        if (moveInput.action != null)
        {
            input = moveInput.action.ReadValue<Vector2>();
        }

        float steer = input.x * maxSteerAngle;
        float motor = input.y * motorForce;

        if (frontLeft) frontLeft.steerAngle = steer;
        if (frontRight) frontRight.steerAngle = steer;

        if (frontLeft) frontLeft.motorTorque = motor;
        if (frontRight) frontRight.motorTorque = motor;
        if (rearLeft) rearLeft.motorTorque = motor;
        if (rearRight) rearRight.motorTorque = motor;
    }

    void Reset()
    {
        // Try to find wheels
        var wheels = GetComponentsInChildren<WheelCollider>();
        foreach (var w in wheels)
        {
            if (w.name.Contains("FrontLeft")) frontLeft = w;
            if (w.name.Contains("FrontRight")) frontRight = w;
            if (w.name.Contains("RearLeft")) rearLeft = w;
            if (w.name.Contains("RearRight")) rearRight = w;
        }

        // Setup default binding for XR Right Controller Joystick
        // Note: In a real project, you'd likely reference an asset, but this creates an internal one for simplicity
        if (moveInput.action == null)
        {
            var action = new InputAction("Move", type: InputActionType.Value, expectedControlType: "Vector2");
            action.AddBinding("<XRController>{RightHand}/thumbstick");
            action.AddBinding("<Gamepad>/leftStick");
            moveInput = new InputActionProperty(action);
        }
    }
}

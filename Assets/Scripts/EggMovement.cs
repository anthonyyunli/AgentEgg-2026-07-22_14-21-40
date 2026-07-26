using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class EggRolling : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GroundSensor groundSensor;
    [SerializeField] private MouseGrabber mouseGrabber;

    [Header("Rolling")]
    [SerializeField] private float torqueStrength = 20f;
    [SerializeField] private float maxAngularSpeed = 80f;
    [SerializeField] private float carryTorqueMultiplier = 0.55f;
    [SerializeField] private float carryAngularMultiplier = 0.65f;

    [Header("Jump")]
    [SerializeField] private float jumpImpulse = 6f;

    private Rigidbody body;
    private Vector2 moveInput;
    private bool jumpRequested;
    private bool isGrounded;
    private Vector3 groundNormal;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.maxAngularVelocity = maxAngularSpeed;

        if (groundSensor == null) groundSensor = GetComponent<GroundSensor>();
        if (mouseGrabber == null) mouseGrabber = GetComponent<MouseGrabber>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && !IsCarrying) jumpRequested = true;
    }

    private void FixedUpdate()
    {
        isGrounded = groundSensor.IsGrounded(out RaycastHit groundHit);

        groundNormal = isGrounded ? groundHit.normal : Vector3.up;
        bool carrying = IsCarrying;
        body.maxAngularVelocity = maxAngularSpeed * (carrying ? carryAngularMultiplier : 1f);

        // Rolling
        if (cameraTransform && moveInput.sqrMagnitude > 0.01f)
        {
            Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 cameraRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

            Vector3 moveDir = cameraForward * moveInput.y + cameraRight * moveInput.x;
            if (isGrounded) moveDir = Vector3.ProjectOnPlane(moveDir, groundNormal);

            moveDir.Normalize();

            Vector3 torqueAxis = Vector3.Cross(groundNormal, moveDir); // roll on axis perpendicular to movement
            body.AddTorque(torqueAxis * torqueStrength * (carrying ? carryTorqueMultiplier : 1f), ForceMode.Acceleration);
        }

        // Jump
        if (jumpRequested && isGrounded && !carrying)
        {
            body.AddForce(groundNormal * jumpImpulse, ForceMode.Impulse);
        }

        jumpRequested = false;
    }

    private bool IsCarrying => mouseGrabber != null && mouseGrabber.IsHolding;
}

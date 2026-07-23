using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class EggRolling : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GroundSensor groundSensor;

    [Header("Rolling")]
    [SerializeField] private float torqueStrength = 20f;
    [SerializeField] private float maxAngularSpeed = 25f;

    [Header("Jump")]
    [SerializeField] private float jumpImpulse = 6f;

    private Rigidbody body;
    private Vector2 moveInput;
    private bool jumpRequested;
    private bool isGrounded;

    private void Awake()
    {
        jumpImpulse = 6f;

        body = GetComponent<Rigidbody>();
        body.maxAngularVelocity = maxAngularSpeed;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);
    }

    public void OnJump(InputValue value)
    {
        Debug.Log("JUMP");
        if (value.isPressed) jumpRequested = true;
    }

    private void FixedUpdate()
    {
        isGrounded = groundSensor.IsGrounded();

        // Rolling
        if (cameraTransform && moveInput.sqrMagnitude > 0.01f)
        {
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            Vector3 moveDir = cameraForward * moveInput.y + cameraRight * moveInput.x;
            moveDir.Normalize();

            Vector3 torqueAxis = Vector3.Cross(Vector3.up, moveDir); // roll on axis perpendicular to movement
            body.AddTorque(torqueAxis * torqueStrength, ForceMode.Acceleration);
        }

        if (jumpRequested && isGrounded)
        {
            body.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
        }

        jumpRequested = false;
        isGrounded = false;
    }

}

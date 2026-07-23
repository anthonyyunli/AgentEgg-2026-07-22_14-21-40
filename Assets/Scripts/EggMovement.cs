using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class EggRolling : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Rolling")]
    [SerializeField] private float torqueStrength = 20f;
    [SerializeField] private float maxAngularSpeed = 25f;

    [Header("Jump")]
    [SerializeField] private float jumpImpulse = 6f;

    [Header("GroundCheck")]
    [SerializeField] private LayerMask groundMask;
     [SerializeField] private float groundCastRadius = 0.45f;
    [SerializeField] private float groundCastDistance = 0.20f;
    [SerializeField] private float groundCastStartOffset = 0.05f;

    [Header("Debug")]
    [SerializeField] private bool isGrounded;

    private Rigidbody body;
    private Vector2 moveInput;
    private bool jumpRequested;

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
        isGrounded = CheckGrounded();

        // Rolling
        if (cameraTransform && moveInput.sqrMagnitude > 0.01f) {
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            Vector3 moveDir = cameraForward * moveInput.y + cameraRight * moveInput.x;
            moveDir.Normalize();

            Vector3 torqueAxis = Vector3.Cross(Vector3.up, moveDir); // roll on axis perpendicular to movement
            body.AddTorque(torqueAxis * torqueStrength, ForceMode.Acceleration);
        }

        if (jumpRequested && isGrounded)
        {
            body.AddForce( Vector3.up * jumpImpulse, ForceMode.Impulse);
        }

        jumpRequested = false;
        isGrounded = false;
    }

    private bool CheckGrounded()
    {
        Vector3 castOrigin = transform.position + Vector3.up * groundCastStartOffset;
        Debug.Log("EE");
        return Physics.SphereCast(
            castOrigin,
            groundCastRadius,
            Vector3.down,
            out _,
            groundCastDistance,
            groundMask,
            QueryTriggerInteraction.Ignore
        );
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 castOrigin = transform.position + Vector3.up * groundCastStartOffset;
        Vector3 castEnd = castOrigin + Vector3.down * groundCastDistance;

        Gizmos.DrawLine(castOrigin, castEnd);
        Gizmos.DrawWireSphere(castEnd, groundCastRadius);
    }
}

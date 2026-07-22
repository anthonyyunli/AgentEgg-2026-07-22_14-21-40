using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class EggRolling : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Movement")]
    [SerializeField] private float torqueStrength = 20f;
    [SerializeField] private float maxAngularSpeed = 25f;

    private Rigidbody body;
    private Vector2 moveInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.maxAngularVelocity = maxAngularSpeed;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);
    }

    private void FixedUpdate()
    {
        if (cameraTransform == null) return;

        if (moveInput.sqrMagnitude < 0.01f) return;

        // Get the camera's horizontal directions.
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Ignore the camera's vertical tilt.
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Convert WASD input into a direction relative to the camera.
        Vector3 moveDirection =
            cameraForward * moveInput.y +
            cameraRight * moveInput.x;

        moveDirection.Normalize();

        // An object rolls around an axis perpendicular
        // to its movement direction.
        Vector3 torqueAxis =
            Vector3.Cross(Vector3.up, moveDirection);

        body.AddTorque(
            torqueAxis * torqueStrength,
            ForceMode.Acceleration
        );
    }
}

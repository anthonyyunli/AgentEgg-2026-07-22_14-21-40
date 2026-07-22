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
        if (cameraTransform==null) return;
        if (moveInput.sqrMagnitude < 0.01f) return;

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        Vector3 moveDir = cameraForward * -moveInput.y + cameraRight * -moveInput.x;
        moveDir.Normalize();

        Vector3 torqueAxis = Vector3.Cross(moveDir, Vector3.up); // roll on axis perpendicular to movement
        body.AddTorque(torqueAxis * torqueStrength, ForceMode.Acceleration);
    }
}

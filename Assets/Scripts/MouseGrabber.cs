using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MouseGrabber : MonoBehaviour
{
    [SerializeField] private float range = 8f;
    [SerializeField] private float breakRange = 10f;
    [SerializeField] private float pullStrength = 120f;
    [SerializeField] private float pullDamping = 18f;
    [SerializeField] private float maxForce = 600f;
    [SerializeField] private float grabWobbleStrength = 0.08f;
    [SerializeField] private float maxGrabWobble = 2.0f;

    private Rigidbody eggBody;
    private Rigidbody heldBody;
    private Vector3 localGrabPoint;
    private float grabDepth;

    public bool IsHolding => heldBody != null;
    public float HeldMass => heldBody ? heldBody.mass : 0f;

    private void Awake()
    {
        eggBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame) Grab(mouse.position.ReadValue());
        if (mouse.leftButton.wasReleasedThisFrame) heldBody = null;
    }

    private void FixedUpdate()
    {
        Mouse mouse = Mouse.current;
        if (heldBody == null || mouse == null || !mouse.leftButton.isPressed) return;

        if (Vector3.Distance(transform.position, heldBody.worldCenterOfMass) > breakRange)
        {
            heldBody = null;
            return;
        }

        Camera cam = Camera.main;
        if (cam == null) return;

        Vector2 mousePosition = mouse.position.ReadValue();
        Vector3 targetPosition = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, grabDepth));
        Vector3 grabPosition = heldBody.transform.TransformPoint(localGrabPoint);
        Vector3 grabVelocity = heldBody.GetPointVelocity(grabPosition);
        Vector3 force = (targetPosition - grabPosition) * pullStrength - grabVelocity * pullDamping;

        if (force.sqrMagnitude > maxForce * maxForce) force = force.normalized * maxForce;
        heldBody.AddForceAtPosition(force, grabPosition, ForceMode.Force);
        AddGrabWobble(heldBody.linearVelocity);
    }

    private void Grab(Vector2 mousePosition)
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f)) return;
        if (!hit.collider.gameObject.CompareTag("Moveable")) return;
        if (Vector3.Distance(transform.position, hit.point) > range) return;

        Rigidbody targetBody = hit.collider.attachedRigidbody;
        if (targetBody == null)
        {
            targetBody = hit.collider.gameObject.AddComponent<Rigidbody>();
            targetBody.mass = EstimateMass(hit.collider.bounds);
        }

        heldBody = null;
        heldBody = targetBody;
        localGrabPoint = heldBody.transform.InverseTransformPoint(hit.point);
        grabDepth = cam.WorldToScreenPoint(hit.point).z;

        heldBody.WakeUp();
    }

    private void AddGrabWobble(Vector3 itemVelocity)
    {
        Vector3 wobble = Vector3.Cross(Vector3.up, itemVelocity - eggBody.linearVelocity) * HeldMass * grabWobbleStrength;
        wobble = Vector3.ProjectOnPlane(wobble, Vector3.up);
        if (wobble.sqrMagnitude < 0.001f) return;

        wobble = Vector3.ClampMagnitude(wobble, maxGrabWobble);
        // Debug.DrawRay(eggBody.worldCenterOfMass, wobble, Color.cyan);
        // Debug.DrawRay(eggBody.worldCenterOfMass, Vector3.Cross(wobble, Vector3.up), Color.yellow);
        // Debug.DrawRay(heldBody.worldCenterOfMass, itemVelocity-eggBody.linearVelocity, Color.red);
        eggBody.AddTorque(wobble, ForceMode.Force);
    }

    private float EstimateMass(Bounds bounds)
    {
        Vector3 size = bounds.size;
        return Mathf.Clamp(size.x * size.y * size.z, 0.2f, 20f);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MouseGrabber : MonoBehaviour
{
    [SerializeField] private float range = 30.5f;
    [SerializeField] private float breakRange = 40f;
    [SerializeField] private float pullStrength = 120f;
    [SerializeField] private float pullDamping = 18f;
    [SerializeField] private float maxForce = 600f;
    [SerializeField] private float eggTugStrength = 18f;
    [SerializeField] private float maxEggTug = 8f;

    private Rigidbody eggBody;
    private Rigidbody heldBody;
    private Vector3 localGrabPoint;
    private float grabDepth;

    private void Awake()
    {
        eggBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame) Grab(mouse.position.ReadValue());
        if (mouse.leftButton.wasReleasedThisFrame) Release();
    }

    private void FixedUpdate()
    {
        Mouse mouse = Mouse.current;
        if (heldBody == null || mouse == null || !mouse.leftButton.isPressed) return;

        if (Vector3.Distance(transform.position, heldBody.worldCenterOfMass) > breakRange)
        {
            Release();
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
        TugEgg();
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
        if (targetBody == null) targetBody = hit.collider.gameObject.AddComponent<Rigidbody>();

        Release();
        heldBody = targetBody;
        localGrabPoint = heldBody.transform.InverseTransformPoint(hit.point);
        grabDepth = cam.WorldToScreenPoint(hit.point).z;

        heldBody.WakeUp();
    }

    private void TugEgg()
    {
        Vector3 tug = heldBody.worldCenterOfMass - eggBody.worldCenterOfMass;
        tug.y = 0f;

        if (tug.sqrMagnitude < 0.01f) return;

        tug = Vector3.ClampMagnitude(tug * eggTugStrength, maxEggTug);
        eggBody.AddForce(tug, ForceMode.Force);
    }

    private void Release()
    {
        heldBody = null;
    }
}

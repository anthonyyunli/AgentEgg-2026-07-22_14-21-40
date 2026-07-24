using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float castRadius = 0.45f;
    [SerializeField] private float castDistance = 0.25f;
    [SerializeField] private float startOffset = 0.1f;

    public bool IsGrounded(out RaycastHit hit)
    {
        Vector3 origin = transform.position + Vector3.up * startOffset;

        return Physics.SphereCast(origin, castRadius, Vector3.down, out hit, castDistance, groundMask, QueryTriggerInteraction.Ignore);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * startOffset;
        Vector3 end = origin + Vector3.down * castDistance;

        Gizmos.DrawLine(origin, end);
        Gizmos.DrawWireSphere(end, castRadius);
    }
}

using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float checkOffset = 0.5f;
    [SerializeField] private float checkRadius = 0.2f;

    public bool IsGrounded()
    {
        Vector3 checkPosition = transform.position + Vector3.down * checkOffset;

        return Physics.CheckSphere(checkPosition, checkRadius, groundMask, QueryTriggerInteraction.Ignore);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 checkPosition = transform.position + Vector3.up * checkOffset;

        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(checkPosition, checkRadius);
    }
}

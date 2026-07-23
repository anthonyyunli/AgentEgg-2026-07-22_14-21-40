using UnityEngine;

public class StickyScript : MonoBehaviour
{
    public Transform player;
    [SerializeField] private LayerMask stickyMask;
    [SerializeField] private float checkPosition;
    [SerializeField] private float checkRadius;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        checkRadius = 1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("is sticky?: "+isSticky());
    }

    public bool isSticky()
    {
        Vector3 checkPosition = player.position;

        return Physics.CheckSphere(checkPosition, checkRadius, stickyMask, QueryTriggerInteraction.Ignore);


    }
}

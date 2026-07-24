using UnityEngine;

public class StickyScript : MonoBehaviour
{
    public Transform player;
    [SerializeField] private LayerMask stickyMask;
    [SerializeField] private float checkPosition;
    [SerializeField] private float checkRadius;

    public Transform honeyPrefab;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        checkRadius = 1f;

   //     Physics.IgnoreCollision(honeyPrefab.GetComponent().GetChild(0).transform.GetComponent<Collider>(), GetComponent<Collider>()); 
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

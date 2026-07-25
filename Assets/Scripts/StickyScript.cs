using UnityEngine;
using UnityEngine.InputSystem;

public class StickyScript : MonoBehaviour
{
    public Transform player;
    [SerializeField] private LayerMask stickyMask;

    [SerializeField] private LayerMask moveableMask;

    [SerializeField] private float checkPosition;
    [SerializeField] private float checkRadius;

    public Transform honeyPrefab;

    public bool sticky = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sticky = false;
   //     Physics.IgnoreCollision(honeyPrefab.GetComponent().GetChild(0).transform.GetComponent<Collider>(), GetComponent<Collider>()); 
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Debug.Log("is sticky?: "+isSticky()+ "Is In Range?"+ isInRange());
        
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame && IsSticky() && IsInRange())
        {
        //connect spring between player and said object
        Debug.Log("Initiate spring");
        
        
        }

    }
    public bool IsSticky()
    {
        Vector3 position = player.position;

        sticky = Physics.CheckSphere(
            position,
            1f,
            stickyMask,
            QueryTriggerInteraction.Ignore
        );

        return sticky;
    }

    public bool IsInRange()
    {
        Vector3 position = player.position;

        return Physics.CheckSphere(
            position,
            2f,
            moveableMask,
            QueryTriggerInteraction.Ignore
        );
    }
}

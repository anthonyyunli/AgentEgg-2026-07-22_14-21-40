using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class EggHealth : MonoBehaviour
{
    //public double playerVelocity;
    public Rigidbody player;
    public float health = 100;
    public float softness = 1;

    public bool isGrounded;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCastRadius = 0.45f;
    [SerializeField] private float groundCastDistance = 0.20f;
    [SerializeField] private float groundCastStartOffset = 0.05f;

    //  public LayerMask groundlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

    // Update is called once per frame
    void Update()
    {

        isGrounded = CheckGrounded();
        // playerVelocity = player.Transform.velocity.y.magnitude;

        if (player.linearVelocity.y * softness * -1 >= 3 && isGrounded)
        { 
            health -= Mathf.Abs(player.linearVelocity.y) - 3; 
        }


        Debug.Log("velocity:" + player.linearVelocity.y + ", health:" + health);
    }
}

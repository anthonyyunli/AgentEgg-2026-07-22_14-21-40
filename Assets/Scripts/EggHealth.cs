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

    public bool isGrounded1;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCastRadius = 0.45f;
    [SerializeField] private float groundCastDistance = 0.20f;
    [SerializeField] private float groundCastStartOffset = 0.5f;

    //  public LayerMask groundlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        isGrounded1 = isGrounded();//only for testing pourposes. delte afterwards

        Debug.Log(isGrounded());

        if (player.linearVelocity.y * softness * -1*Time.deltaTime*60 >= 3 && isGrounded())
        { 
            health -= Mathf.Abs(player.linearVelocity.y)*Time.deltaTime *60- 3; 
        }


       // Debug.Log("velocity:" + player.linearVelocity.y + ", health:" + health);
    }

    private bool isGrounded()
    {
        return Physics.Raycast(player.position, Vector3.down, 0.7f, groundMask);
        
    }
}

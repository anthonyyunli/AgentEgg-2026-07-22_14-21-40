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

    public bool isGrounded1 = true;

    [SerializeField] private LayerMask groundMask;

    //  public LayerMask groundlayer;

    void Awake()
    {

        softness = 1;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        

        Debug.Log(isGrounded());

        //if (player.linearVelocity.y * -1*Time.deltaTime*60 >= 1 && isGrounded())
        if (isGrounded() && !isGrounded1)
        {
            if (Mathf.Abs(player.linearVelocity.y) * softness > 10)
            {
                health -= Mathf.Abs(player.linearVelocity.y) * softness * 6;
            }
            else
            {
                health -= Mathf.Abs(player.linearVelocity.y) * softness;
            }
        }


        // Debug.Log("velocity:" + player.linearVelocity.y + ", health:" + health);
        isGrounded1 = isGrounded();//only for testing pourposes. delte afterwards
    }

    private bool isGrounded()
    {
        return Physics.Raycast(player.position, Vector3.down, 1f, groundMask);
        
    }
}

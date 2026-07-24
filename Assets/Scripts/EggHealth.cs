using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EggHealth : MonoBehaviour
{

    public Slider healthslider;
    public Image healthfill;
    public Gradient healthgradient;

    //public double playerVelocity;
    public Rigidbody player;
    public float health = 100;
    private float softness = 1;
    private bool wasGrounded = true;

    [SerializeField] private float checkOffset = 0.5f;
    [SerializeField] private float checkRadius = 0.4f;

    public GroundSensor groundSensor;

      public LayerMask groundMask;

    void Awake()
    {

        softness = 1;
        setMaxHealth(health);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        

        bool groundedNow = groundSensor.IsGrounded(out _);

        //if (player.linearVelocity.y * -1*Time.deltaTime*60 >= 1 && isGrounded())
        if (groundedNow && !wasGrounded)
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
        wasGrounded = groundedNow;//only for testing pourposes. delte afterwards

        setHealth(health);
    }

    public void setMaxHealth(float health)
    {
        healthslider.maxValue = 100;
        healthslider.value = health;
        healthfill.color = healthgradient.Evaluate(1f);
    }

    public void setHealth(float health)
    {
        healthslider.value = health;
        healthfill.color = healthgradient.Evaluate(health/100);
    }
    // public bool isGrounded()
    // {
    //     Vector3 checkPosition = transform.position + Vector3.down * checkOffset;

    //     return Physics.CheckSphere(checkPosition, checkRadius, groundMask, QueryTriggerInteraction.Ignore);
    // }

}

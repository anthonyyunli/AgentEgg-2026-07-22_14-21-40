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
    public float softness = 1;

    public bool isGrounded1 = true;

    [SerializeField] private LayerMask groundMask;

    //  public LayerMask groundlayer;

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

        setHealth(health);
    }

    private bool isGrounded()
    {
        return Physics.Raycast(player.position, Vector3.down, 1f, groundMask);
        
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


}

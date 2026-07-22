using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class EggHealth : MonoBehaviour
{
    //public double playerVelocity;
    public Rigidbody player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // playerVelocity = player.Transform.velocity.y.magnitude;
        Debug.Log(player.linearVelocity);
    }
}

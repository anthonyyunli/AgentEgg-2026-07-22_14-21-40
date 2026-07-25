using UnityEngine;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;//most of these are probably not needed

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
    void FixedUpdate()
    {
        Debug.Log("is sticky?: "+isSticky()+ "Is In Range?"+ isInRange());
        
        if (Input.GetKeyDown(KeyCode.Q) && isSticky() && isInRange())
        {
        //connect spring between player and said object
        Debug.Log("Initiate spring");
        
        
        }

    }

    public bool isSticky()
    {
        if (sticky) return true;
        else
            {


        Vector3 checkPosition = player.position;

        checkRadius = 1f;

        sticky = Physics.CheckSphere(checkPosition, checkRadius, stickyMask, QueryTriggerInteraction.Ignore);

        return Physics.CheckSphere(checkPosition, checkRadius, stickyMask, QueryTriggerInteraction.Ignore);



        }
    }

    public bool isInRange()
    {


        Vector3 checkPosition = player.position;

        checkRadius = 2f;



        return Physics.CheckSphere(checkPosition, checkRadius, moveableMask, QueryTriggerInteraction.Ignore);


    }
}

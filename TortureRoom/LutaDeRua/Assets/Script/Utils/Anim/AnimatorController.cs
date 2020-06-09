using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IAnimatorController
{
    void Idle(bool p);
    void Walk(bool p);
    void Punch(bool p);
    void Combo();
}

public class AnimatorController : MonoBehaviour, IAnimatorController
{
    //Input
    //Receive input dependencies
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    //Receive anim / Send to execute
    Animator anim;

    //Receive anim dependencies
    public PlayerCollisions pCol1, pCol2;

    //Execute
    //Trigger animation
    public void Idle(bool val)
    {
        //If no key pressed
        if (!anim.GetBool("Walk"))
            anim.SetBool("Idle", val);
        //throw new NotImplementedException();
    }

    public void Walk(bool val)
    {
        if (anim.GetBool("Idle"))
            Idle(false);

        anim.SetBool("Walk", val);
    }

    public void Punch(bool val)
    {
        //Cancels other animation
        if (anim.GetBool("Punch"))
        {
            if (anim.GetBool("Idle"))
                Idle(true);
            if (anim.GetBool("Walk"))
                Walk(false);
        }
            
        //Execute punch
        anim.SetBool("Punch", val);
        fightState = true;
        //fightState = val;
    }

    public void Combo()
    {
        //If punched something 
        anim.SetBool("Combo", hit);
        fightState = hit;
    }

    //Outsiders (Breaking single resp)
    
    [SerializeField] private bool hit = false;  // If hit something enter in combo state and continue punching
    [SerializeField] private bool fightState = false;   //If is fighting cannot walk

    //Output
    //Sent animation
    void Update()
    {
        //if (Input.GetKey(KeyCode.UpArrow))
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            if(anim.GetBool("Idle"))
                Walk(true);
        }
        else
        {
            Walk(false);
            Idle(true);
        }

        Punch(false);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Punch(true);
            Combo();
        }

        //if (Input.GetKeyDown(KeyCode.C))
        if(pCol1.hitTarg || pCol2.hitTarg)
        {
            hit = !hit;
        }

    }










}

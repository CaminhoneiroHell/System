using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCollisions : MonoBehaviour
{
    public bool hitTarg;
    //private void OnTriggerEnter(Collider other)
    //{
    //    hitTarg = true;
    //    print("Touched");
    //}

    private void OnTriggerStay(Collider other)
    {
        hitTarg = true;
        print("touching");
    }

    private void OnTriggerExit(Collider other)
    {
        hitTarg = false;
        print("Leave");
    }
}

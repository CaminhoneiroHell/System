﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMenuState : MonoBehaviour, IFSMState
{
    public IEnumerator Enter()
    {
        GameManager.Instance.isUsingVR = true;
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator Exit()
    {
        yield return new WaitForEndOfFrame();
    }

    public void FSMUpdate(){

    }
}
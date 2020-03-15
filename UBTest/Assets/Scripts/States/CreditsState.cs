using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsState : MonoBehaviour, IFSMState
{

    public IEnumerator Enter()
    {
        yield return new WaitForSeconds(0.9f);
    }

    public IEnumerator Exit()
    {
        yield return new WaitForSeconds(0.9f);
    }

    public void FSMUpdate(){

    }
}
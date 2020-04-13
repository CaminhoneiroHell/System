using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTrackState : MonoBehaviour, IFSMState
{
    public IEnumerator Enter()
    {
        EventManager.StartWithNormalCam();
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator Exit()
    {
        yield return new WaitForEndOfFrame();
    }

    public void FSMUpdate(){

    }
}
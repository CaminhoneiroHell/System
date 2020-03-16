using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectTrackVRState : MonoBehaviour, IFSMState
{
    public IEnumerator Enter()
    {
        EventManager.StartVR();
        SceneManager.LoadScene("VRTrackSelectScreen");
        GameManager.Instance.isUsingVR = true;
        //yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator Exit()
    {
        yield return new WaitForEndOfFrame();
    }

    public void FSMUpdate(){

    }
}
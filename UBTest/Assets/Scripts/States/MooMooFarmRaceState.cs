using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MooMooFarmRaceState : MonoBehaviour, IFSMState
{
    public IEnumerator Enter()
    {
        //yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("FarmRoad");
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator Exit()
    {
        yield return new WaitForEndOfFrame();
    }

    public void FSMUpdate(){

    }
}
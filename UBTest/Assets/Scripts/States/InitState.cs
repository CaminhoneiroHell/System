using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitState : MonoBehaviour, IFSMState
{

    public IEnumerator Enter()
    {
        //Splash Screen
        yield return new WaitForSeconds(1f);
        GameManager.Instance.ChangeState(State.MainScreen);
    }

    public IEnumerator Exit()
    {
        yield return new WaitForSeconds(0.1f);
    }

    public void FSMUpdate(){

    }
}
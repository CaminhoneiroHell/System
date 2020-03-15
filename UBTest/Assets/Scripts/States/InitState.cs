using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitState : MonoBehaviour, IFSMState
{

    public IEnumerator Enter()
    {
        //yield return new WaitForEndOfFrame();
        //SceneManager.LoadScene("InitState");
        yield return new WaitForSeconds(0.9f);
        // GameManager.instance.ChangeState(State.Space);
    }

    public IEnumerator Exit()
    {
        yield return new WaitForSeconds(0.9f);
    }

    public void FSMUpdate(){

    }
}
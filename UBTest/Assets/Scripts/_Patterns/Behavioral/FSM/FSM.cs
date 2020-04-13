using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSM : MonoBehaviour {

	private IFSMState currentState;

	public IEnumerator ChangeState(IFSMState newState){
        StopAllCoroutines();
		if(currentState != null){
			yield return StartCoroutine(currentState.Exit());
		}
		currentState = newState;
		StartCoroutine(currentState.Enter());
	}

	public void Initialize(IFSMState state){
		StartCoroutine(ChangeState(state));
	}

	public IFSMState GetCurrentState(){
		return currentState;
	}

	private void Update(){
		if(currentState != null){
			currentState.FSMUpdate();
		}
	}

	private void DestroyStateOnChange(Object curState)
	{
		Destroy(curState);
	}

}

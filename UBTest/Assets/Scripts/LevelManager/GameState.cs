using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFSMGamestate
{
	void Enter();
	void Intialize(IFSMGamestate data);
    void OnUpdate();
	void Exit();
}

public class FSMGameState: IFSMGamestate
{
	IFSMGamestate _currentState;
	public void Intialize(IFSMGamestate data)
	{
		ChangeState(data);
	}

	public void Enter()
	{
		throw new System.NotImplementedException();
	}

	public void OnUpdate()
	{
		throw new System.NotImplementedException();
	}

	public void Exit()
	{
		throw new System.NotImplementedException();
	}

	public void ChangeState(IFSMGamestate newGameState)
	{
		if(_currentState != null){
			_currentState.Exit();
		}
		_currentState = newGameState;
		_currentState.Enter();
	}

}



//IEnumerator Enter();
////Stater exit
//IEnumerator Exit();
////Everyframe
//void FSMUpdate();

//private IFSMState currentState;

//public IEnumerator ChangeState(IFSMState newState)
//{
//	StopAllCoroutines();
//	if (currentState != null)
//	{
//		yield return StartCoroutine(currentState.Exit());
//	}
//	currentState = newState;
//	StartCoroutine(currentState.Enter());
//}

//public void Initialize(IFSMState state)
//{
//	StartCoroutine(ChangeState(state));
//}

//public IFSMState GetCurrentState()
//{
//	return currentState;
//}

//private void Update()
//{
//	if (currentState != null)
//	{
//		currentState.FSMUpdate();
//	}
//}

//private void DestroyStateOnChange(Object curState)
//{
//	Destroy(curState);
//}

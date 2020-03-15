/* Classe que controla uma maquina de estados
 */

using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour {

	private IFSMState currentState;

	public IEnumerator ChangeState(IFSMState newState){
        StopAllCoroutines();
		//Verifica se o estado atual nao é nulo
		if(currentState != null){
			//Se nao for, damos um exit
			yield return StartCoroutine(currentState.Exit());
		}
		currentState = newState;
		StartCoroutine(currentState.Enter());
	}

	//Metodo para inicializar o FSM, semelhante a um constructor
	public void Initialize(IFSMState state){
		StartCoroutine(ChangeState(state));
	}

	public IFSMState GetCurrentState(){
		return currentState;
	}

	//Atualiza o estado Update dos estados
	private void Update(){
		if(currentState != null){
			currentState.FSMUpdate();
		}
	}
}

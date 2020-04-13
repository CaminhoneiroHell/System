using UnityEngine;
using System.Collections;

public interface IFSMState {
	//State enter
	IEnumerator Enter();
	//Stater exit
	IEnumerator Exit();
	//Everyframe
	void FSMUpdate();
}

/* Interface para os estados que são gerenciados pelo FSM.cs
 */

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

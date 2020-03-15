using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDoorEvent : MonoBehaviour {

    [SerializeField]
    GameObject spaceDoor;
    bool moveDoor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(moveDoor)
            spaceDoor.transform.Rotate(0, -10 * Time.deltaTime, 0);
        else
            spaceDoor.transform.Rotate(0, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "SeriousGameStateManager")
            moveDoor = true;
    }
}

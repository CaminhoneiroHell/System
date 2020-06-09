using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    int doorType = AttributeManager.MAGIC;

    void OnCollisionEnter(Collision other) {
        if((other.gameObject.GetComponent<AttributeManager>().attributes & doorType) != 0){
            this.GetComponent<BoxCollider>().isTrigger = true;
        }    
    }

    void OnTriggerExit(Collider other) {
        this.GetComponent<BoxCollider>().isTrigger = false;        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

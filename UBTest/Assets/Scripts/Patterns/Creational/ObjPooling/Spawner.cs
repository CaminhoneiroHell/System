using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    
    private void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            //Communicate with the obj pool system
            //Request a pooled Obj
            GameObject pooled = PoolManager.Instance.RequestPooledObj();
            pooled.transform.position = Vector3.zero;
        }        
    }
}

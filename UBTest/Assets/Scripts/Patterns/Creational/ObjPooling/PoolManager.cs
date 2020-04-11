using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Creational.Singleton;

public class PoolManager : Singleton<PoolManager>
{
    private GameObject poolObjContainer;
    [SerializeField] GameObject poolObj;
    private List<GameObject> pooledGmObjList = new List<GameObject>();

    private void Start()
    {
        GeneratePooledObj(10);
    }

    public List<GameObject> GeneratePooledObj(int size)
    {
        if(poolObjContainer == null)
        {
            poolObjContainer = new GameObject();
            poolObjContainer.gameObject.name = "PooledObjectsContainer";
        }

        for (int i = 0; i < size; i++){
            GameObject GO = Instantiate(poolObj);
            pooledGmObjList.Add(GO);
            GO.transform.SetParent(poolObjContainer.transform);
            GO.SetActive(false);
        }
        return pooledGmObjList;
    }

    public GameObject RequestPooledObj()
    {
        print("Request obj");
        //bool requestReceived = false;
        //loop through the obj list
        foreach(GameObject gPool in pooledGmObjList)
        {
            //checking for non-active obj
            if (!gPool.activeSelf){
                //found one and set it active and return it to the player
                gPool.SetActive(true);
                return gPool;
            }    
        }
        //if no obj is available (all are turned on)
        print("no obj is available, generating 3 amount of obj");
        //generate x amount of obj and run the request obj method
        GeneratePooledObj(3);
        foreach (GameObject gPool in pooledGmObjList){
            if (!gPool.activeSelf){
                gPool.SetActive(true);
                return gPool;
            }
        }
        return null;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Creational.Singleton;

public class PoolManager : Singleton<PoolManager>
{
    private GameObject p_ObjContainer;
    [SerializeField] GameObject p_Obj;
    private List<GameObject> p_GmObjList = new List<GameObject>();

    private void Start()
    {
        GeneratePooledObj(10);
    }

    public List<GameObject> GeneratePooledObj(int size)
    {
        if(p_ObjContainer == null)
        {
            p_ObjContainer = new GameObject();
            p_ObjContainer.gameObject.name = "PooledObjectsContainer";
        }

        for (int i = 0; i < size; i++){
            GameObject GO = Instantiate(p_Obj);
            p_GmObjList.Add(GO);
            GO.transform.SetParent(p_ObjContainer.transform);
            GO.SetActive(false);
        }
        return p_GmObjList;
    }

    public GameObject RequestPooledObj()
    {
        print("Request obj");
        //bool requestReceived = false;
        //loop through the obj list
        foreach(GameObject gPool in p_GmObjList)
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
        foreach (GameObject gPool in p_GmObjList){
            if (!gPool.activeSelf){
                gPool.SetActive(true);
                return gPool;
            }
        }
        return null;
    }

}

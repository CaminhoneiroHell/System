using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Patterns.Creational.GenericObjectPooling
{ 
    public class GenericObjectPoolingScript : MonoBehaviour {

        public static GenericObjectPoolingScript current;
        public GameObject pooledObject;
        public int pooledAmount = 20;
        public bool willGrow = true;

        public List<GameObject> pooledObjects;

        private void Awake()
        {
            current = this;
        }

        // Use this for initialization
        void Start () {


            pooledObjects = new List<GameObject>();
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                //bullets.Add(bullet);
                GameObject obj = (GameObject)Instantiate(pooledObject);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }

            if(willGrow)
            {
                GameObject obj = Instantiate(pooledObject);
                pooledObjects.Add(obj);
                return obj;
            }

            return null; //Remember nullable object pattern

        }
	
	    // Update is called once per frame
	    void Update () {
		
	    }
    }
}
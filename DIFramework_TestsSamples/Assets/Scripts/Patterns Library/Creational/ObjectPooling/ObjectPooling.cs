using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Patterns.Creational.ObjectPooling
{ 

    public class ObjectPoolingScript : MonoBehaviour {
        // Use this for initialization
        void Start () {
		
	    }
	
	    // Update is called once per frame
	    void Update () {
	    }
    }


    public class ObjectDestroyScript: MonoBehaviour
    {
        private void OnEnable()
        {
            Invoke("Destroy", 2f);
        }

        private void Destroy()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
    }

    public class ObjectFireScript: MonoBehaviour
    {

        public float fireTime = 0.5f;
        public GameObject bullet;

        public int pooledAmount = 20;
        List<GameObject> bullets;

        private void Start()
        {
            //bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);


            bullets = new List<GameObject>();
            for (int i = 0; i < pooledAmount; i++)
            {
                //bullets.Add(bullet);
                GameObject obj = (GameObject)Instantiate(bullet);
                obj.SetActive(false);
                bullets.Add(obj);
            }

            InvokeRepeating("Fire", fireTime, fireTime);
        }
    
        public void Fire()
        {
            for(int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].activeInHierarchy)
                {
                    bullets[i].transform.position = transform.position;
                    bullets[i].transform.rotation = transform.rotation;
                    bullets[i].SetActive(true);
                    break;
                }
            }
        }
    
    }

}
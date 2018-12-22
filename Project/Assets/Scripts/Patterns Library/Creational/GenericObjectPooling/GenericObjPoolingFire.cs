using UnityEngine;
using Patterns.Creational.GenericObjectPooling;

public class GenericObjPoolingFire : MonoBehaviour {
    public float fireTime;
	void Start ()
    {
        InvokeRepeating("Fire", fireTime, fireTime);
    }

    void Fire()
    {
        GameObject obj = GenericObjectPoolingScript.current.GetPooledObject();
        if (obj == null) return; 

        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
    }
	
	void Update () {
		
	}
}

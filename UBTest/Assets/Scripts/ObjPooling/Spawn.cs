using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public GameObject asteroid;

    // Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

        if (Random.Range(0, 100) < 10)
        {
            //Instantiate(asteroid,
            //            this.transform.position +
            //            new Vector3(Random.Range(-10, 10), 0, 0), Quaternion.identity);
           
            GameObject a = Pool.singleton.Get("asteroid");
            if (a != null)
            {
                a.transform.position = this.transform.position + 
                                       new Vector3(Random.Range(-5, 5), 0, 0);
                a.SetActive(true);
            }
        }
    }
}

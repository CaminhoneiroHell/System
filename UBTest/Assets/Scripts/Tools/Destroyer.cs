using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {
    [SerializeField]
    float timer;

	void Start () {
        Destroy(gameObject, timer);
	}
}

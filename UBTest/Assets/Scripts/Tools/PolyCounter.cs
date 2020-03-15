using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyCounter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("O total de triangulos nesse gmobj é de: " +
            this.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length / 3);
    }
}

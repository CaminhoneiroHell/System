using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 100) < 10)
            ProcCube.Clone(this.transform.position);
    }
}

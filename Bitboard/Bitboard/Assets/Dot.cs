using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [SerializeField] GameObject cube1, cube2;

    public float dot = 0;

    //public Transform other;

    void Update()
    {
        Vector3 forward = cube1.transform.TransformDirection(Vector3.forward);
        Vector3 toOther = cube2.transform.position - cube1.transform.position;
        
        dot = Vector3.Dot(forward, toOther);
        
        if (dot < 0)
        {
            print("The other transform is behind me!");
        }

    }
}

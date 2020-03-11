using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformation : MonoBehaviour
{

    public GameObject point;
    public float angle;
    public Vector3 translation; 

    void Start()
    {
        Coords position = new Coords(point.transform.position, 1);
        point.transform.position = HolisticMath.Translate(position, new Coords(new Vector3(translation.x,
                                                                                           translation.y,
                                                                                           translation.z), 0)).ToVector();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

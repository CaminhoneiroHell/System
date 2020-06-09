using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFollow : MonoBehaviour
{
    //[SerializeField] Vector3 handL_Col, handR_Col;
    [SerializeField] Transform handL, handR, handL_Col, handR_Col;

    void Update()
    {
        handL_Col.position = new Vector3(handL.position.x, handL.position.y, handL.position.z);
        handR_Col.position = new Vector3(handR.position.x, handR.position.y, handR.position.z);
    }
}

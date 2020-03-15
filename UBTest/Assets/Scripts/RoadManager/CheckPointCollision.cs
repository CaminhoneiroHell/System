using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointCollision : MonoBehaviour
{
    public RoadManager roadManager;

    private void Start()
    {
        roadManager = GetComponentInParent<RoadManager>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        roadManager.CheckPointReached();

    }
}

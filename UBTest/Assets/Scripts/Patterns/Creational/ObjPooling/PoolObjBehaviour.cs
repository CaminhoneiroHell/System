using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjBehaviour : MonoBehaviour
{
    private void Start()
    {
        Invoke("Hide", 3f);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

}

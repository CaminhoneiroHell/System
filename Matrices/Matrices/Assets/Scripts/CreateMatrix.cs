﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMatrix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float[] mvals = { 1, 2, 3, 4, 5, 6};
        Matrix m = new Matrix(2, 3, mvals);
        // Debug.Log(m.ToString());

        
        float[] nvals = { 1, 2, 3, 4, 5, 6};
        Matrix n = new Matrix(3, 2, nvals);


        Matrix asnwer = m * n;

        Debug.Log(asnwer.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

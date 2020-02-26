using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBits : MonoBehaviour
{

    public static int MAGIC = 16;
    public static int INT = 8;
    public static int CHARISMA = 4;
    public static int FLY = 2;
    public static int INVISIBLE = 1;
    //Flag
    int charAttributes = CHARISMA | MAGIC;
    [SerializeField] int bitSeq;
    int pack = 0;
    void Start()
    {
        //Set new flag
        charAttributes |= INT;
        //Unsetting flag
        charAttributes &= ~MAGIC;

        // bitSeq = bitSeq >> 2;

        // pack = bitSeq | (bitSeq >> 1);a
    }


    void Update()
    {
        Debug.Log(Convert.ToString(bitSeq, 2).PadLeft(8, '0'));
        
    }
}

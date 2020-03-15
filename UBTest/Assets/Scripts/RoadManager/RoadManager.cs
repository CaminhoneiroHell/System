using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] int curCheckPoint, lastCheckPoint;
    [SerializeField] int checkPointCounter;
    [SerializeField] static int lap = 1;
    [SerializeField] static int totalLapsToFinish = 3;
    
    public GameObject[] checkPoints;
    public GameObject racer;

    private void Start()
    {
        curCheckPoint = 0;
        lastCheckPoint = checkPoints.Length - 1;

    }

    public void CheckPointReached()
    {
        print("Congrats you passed through the: " + (curCheckPoint + 1) + "th CheckPoint.");
        checkPointCounter += 1;
        curCheckPoint = checkPointCounter;
        if(curCheckPoint == checkPoints.Length)
        {
            lap += 1;   //Add lap
            curCheckPoint = 0;//Reset checkPoint back to 0
            print("Lap: " + lap);
            if(lap >= totalLapsToFinish)
            {
                print("Map finished");
            }
        }
    }



}

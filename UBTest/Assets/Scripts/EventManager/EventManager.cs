using System;
using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    public static Action onStartGame;
    public static Action onStartVR;

    public delegate void StartNormalCamDelegate();
    public static StartNormalCamDelegate onStartNormalCam;

    public delegate void StartRaceDelegate();
    public static StartRaceDelegate onStartRace;

    public delegate void PauseDelegate();
    public static PauseDelegate onPause;

    public delegate void TakeDamageDelegate(float amt);
    public static TakeDamageDelegate onTakeDamage;

    public delegate void UnlockStageDelegate(bool locked, int stage);
    public static UnlockStageDelegate unlockStage;

    public delegate void RaceFinisherDelegate();
    public static RaceFinisherDelegate onRaceFinished;

    public static void PauseGame()
    {
        Debug.Log("Pause event called");
        if (onPause != null)
            onPause();
    }

    public static void RaceFinished()
    {
        Debug.Log("Race finished");
        if (onRaceFinished != null)
            onRaceFinished();
    }

    public static void StartWithNormalCam()
    {
        Debug.Log("Normal cam start");
        if (onStartNormalCam != null)
            onStartNormalCam();
    }

    public static void StartVR()
    {
        Debug.Log("Start VR");
        if (onStartVR != null)
            onStartVR();
    }
 
    public static void StartGame()
    {
        Debug.Log("StartGame");
        if (onStartGame != null)
            onStartGame();
    }

    public static void StartRace()
    {
        Debug.Log("Start Race!");
        if (onStartRace != null)
            onStartRace();
    }

    public static void TakeDamage(float percent)
    {
        Debug.Log("TakeDamage" + percent);
        if (onTakeDamage != null)
            onTakeDamage(percent);
    }

    public static void UnlockStage(bool locker, int stg)
    {
        Debug.Log("Unlocked Stage: " + stg + " "+ locker);
        if (unlockStage != null)
            unlockStage(locker, stg);
    }


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

}
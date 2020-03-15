using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadManager : MonoBehaviour
{
    [SerializeField] int curCheckPoint, lastCheckPoint;
    [SerializeField] int checkPointCounter;
    [SerializeField] int lap = 1;
    [SerializeField] int totalLapsToFinish = 3;


    public CameraFollow360 cam360;

    public GameObject[] checkPoints;
    public GameObject racer;
    public PlayerController playerController;
    public Image sempaphoreLed1, sempaphoreLed2, sempaphoreLed3;
    public GameObject semaphore;

    float timer = 0.0f; int seconds;

    //public SumScoreExample scoreManager;

    void OnEnable()
    {
        //EventManager.onStartRace += ...
        EventManager.onRaceFinished += SelectScene; //EndLevel;
    }

    void OnDisable()
    {
        EventManager.onRaceFinished -= SelectScene;//EndLevel;
    }

    //bool isVR = false;
    void SelectScene()
    {
        if (GameManager.Instance.isUsingVR)
            GameManager.Instance.ChangeState(State.SelectTrackVR);
        else
            GameManager.Instance.ChangeState(State.SelectTrack);
    }

    bool toogleClock = false;
    void TimerCounter()
    {
        timer += Time.deltaTime;
        seconds = (int)(timer % 60);
    }

    void ResetTimer()
    {
        timer = 0.0f;
    }

    [Header("Enable intro cutscene before start")]
    [SerializeField] bool enableIntro = false;
    private void Start()
    {
        curCheckPoint = 0;
        lastCheckPoint = checkPoints.Length - 1;
        EventManager.StartRace();
        cam360.GetComponent<CameraFollow360>();
        //scoreManager.GetComponent<SumScoreExample>();

        if (enableIntro)
        {
            cam360.distance = -50;
            playerController.GetComponent<PlayerController>().enabled = false;
        }

    }

    private void Update()
    {
        if (enableIntro)
        {
            if (cam360.distance < 35)
            {
                cam360.distance += 8.5f * Time.deltaTime;
            }

            if (cam360.distance > 20 && !semaphoreStartCount)
            {
                semaphoreStartCount = true;
                StartCoroutine(Semaphore());
            }
        }

        if (toogleClock)
            TimerCounter();
    }

    bool semaphoreStartCount;
    IEnumerator Semaphore()
    {
        //scoreManager.ToggleTimed();
        //semaphoreStartCount = true;
        print("Red");
        sempaphoreLed1.color = Color.red;
        yield return new WaitForSeconds(1f);
        print("3");
        yield return new WaitForSeconds(1f);
        print("2");
        print("Yellow");
        sempaphoreLed2.color = Color.yellow;
        yield return new WaitForSeconds(1f);
        print("1");
        yield return new WaitForSeconds(1f);
        print("Green");
        sempaphoreLed3.color = Color.green;
        playerController.enabled = true;
        print("Go!!!!");
        yield return new WaitForSeconds(1f);
        semaphore.SetActive(false);
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
            checkPointCounter = 0;
            print("Lap: " + lap);
            if(lap == totalLapsToFinish)
            {
                print("Final Lap");
            }
        }

        if (lap > totalLapsToFinish)
            EventManager.RaceFinished();
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Creational.Singleton;

public enum GAMESTATUS
{
    INIT,
    RUNNING,
    PAUSED
}

public class GameManager : Singleton<GameManager> {

    public GAMESTATUS gmStats;
    //private SecretConsole secretConsole;

    //States
    private InitState initState;
    private MainScreenState mainScreenState;
    private MenuState menuState;
    private VRMenuState vrMenuState;
    private SelectTrackState selectTrackState;
    private SelectTrackVRState selectTrackVRState;
    private MooMooFarmRaceState mooMooFarmState;
    private UnderwaterRaceState underWaterState;
    private RainbowRoadRaceState rainbowRoadState;
    private StatsState statsState;
    private CreditsState creditsState;

    //Strategy
    private FSM fsm;
    public State currentState { get; private set; } //Stores actual state

    public GAMESTATUS isPaused { get; private set; } // Terrible I know 
    public bool isUsingVR { get; set; }

    private void Start()
    {
        isUsingVR = false;
        //Dont let this object be destroyed
        DontDestroyOnLoad(gameObject);
        //Create machine state
        fsm = gameObject.AddComponent<FSM>();
        //Define states
        initState = gameObject.AddComponent<InitState>();
        mainScreenState = gameObject.AddComponent<MainScreenState>();
        menuState = gameObject.AddComponent<MenuState>();
        vrMenuState = gameObject.AddComponent<VRMenuState>();
        selectTrackState = gameObject.AddComponent<SelectTrackState>();
        selectTrackVRState = gameObject.AddComponent<SelectTrackVRState>();
        mooMooFarmState = gameObject.AddComponent<MooMooFarmRaceState>();
        underWaterState = gameObject.AddComponent<UnderwaterRaceState>();
        rainbowRoadState = gameObject.AddComponent<RainbowRoadRaceState>();
        statsState = gameObject.AddComponent<StatsState>();
        creditsState = gameObject.AddComponent<CreditsState>();

        //Initialize fsm putting in the first state
        fsm.Initialize(initState);
    }

    public void ChangeState(State newState)
    {
        if (currentState == newState)
            return; 

        Debug.Log("Changing State To: " + (State)newState);

        switch (newState)
        {
            case State.Init:
                StartCoroutine(fsm.ChangeState(initState));
                break;
            case State.MainScreen:
                StartCoroutine(fsm.ChangeState(mainScreenState));
                break;
            case State.Menu:
                StartCoroutine(fsm.ChangeState(menuState));
                break;
            case State.VRMenu:
                StartCoroutine(fsm.ChangeState(vrMenuState));
                break;
            case State.SelectTrack:
                StartCoroutine(fsm.ChangeState(selectTrackState));
                break;
            case State.SelectTrackVR:
                StartCoroutine(fsm.ChangeState(selectTrackVRState));
                break;
            case State.MooMooFarm_race:
                StartCoroutine(fsm.ChangeState(mooMooFarmState));
                break;
            case State.Underwater_race:
                StartCoroutine(fsm.ChangeState(underWaterState));
                break;
            case State.RainbowRoad_race:
                StartCoroutine(fsm.ChangeState(rainbowRoadState));
                break;
            case State.Stats:
                StartCoroutine(fsm.ChangeState(statsState));
                break;
            case State.Credits:
                StartCoroutine(fsm.ChangeState(creditsState));
                break;
        }
        currentState = newState;
    }

    private void GamePauseToogle(GAMESTATUS gm)
    {
        gmStats = gm;
        switch (gmStats)
        {
            case GAMESTATUS.PAUSED:
                Time.timeScale = 0;
                System.GC.Collect();
                break;
            case GAMESTATUS.RUNNING:
                Time.timeScale = 1;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        //print(Time.timeScale);
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            if(gmStats != GAMESTATUS.PAUSED)
            {
                GamePauseToogle(GAMESTATUS.PAUSED); 
                print("PAUSED");
            }
            else
            {
                GamePauseToogle(GAMESTATUS.RUNNING); 
                print("RUNNING");
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
            Time.timeScale = 1;
        if (Input.GetKeyDown(KeyCode.F2))
            Time.timeScale = 2;
        if (Input.GetKeyDown(KeyCode.F3))
            Time.timeScale = 3;
        if (Input.GetKeyDown(KeyCode.F4))
            Time.timeScale = 4;
    }
}

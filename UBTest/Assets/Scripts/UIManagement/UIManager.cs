using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject splashScreen;

    public GameObject pauseMenu;

    public GameObject gameModeSelect;

    public GameObject trackSelectNonVR;

    bool isDisplayed = true;

    void OnEnable()
    {
        EventManager.onStartGame += HideSplashScreen;
        EventManager.onStartNormalCam += SelectTrackMode;
    }

    void OnDisable()
    {
        EventManager.onStartGame -= HideSplashScreen;
        EventManager.onStartNormalCam -= SelectTrackMode;
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    //UI Logic toogled by events

    void HideSplashScreen()
    {
        isDisplayed = !isDisplayed;
        splashScreen.SetActive(isDisplayed);
        gameModeSelect.SetActive(true);
    }

    void SelectTrackMode()
    {
        gameModeSelect.SetActive(false);
        trackSelectNonVR.SetActive(true);

    }

    //UI Logic by Input
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.currentState == State.MainScreen)
            PlayGame();
    }

    public void PlayGame()
    {
        EventManager.StartGame();
    }

    //UI Logic by UIButton toogling
    public void EnterVRGameplayMode()
    {
        GameManager.Instance.ChangeState(State.SelectTrackVR);
    }

    public void EnterNormalGameplayMode()
    {
        GameManager.Instance.ChangeState(State.SelectTrack);
    }
    



}

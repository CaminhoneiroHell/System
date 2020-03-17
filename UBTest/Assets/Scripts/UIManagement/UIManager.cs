using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject splashScreen;
    public GameObject pauseMenu;
    public GameObject gameModeSelect;
    public GameObject trackSelectNonVR;
    public GameObject dummyCamera;
    public Text vrAdiviseText;
    bool isDisplayed = true;

    void OnEnable()
    {
        EventManager.onStartGame += HideSplashScreen;
        EventManager.onStartNormalCam += SelectTrackMode;
        EventManager.onStartRace += HideTrackSelect;
        EventManager.onStartVR += EnterVRMode;
    }

    void OnDisable()
    {
        EventManager.onStartGame -= HideSplashScreen;
        EventManager.onStartNormalCam -= SelectTrackMode;
        EventManager.onStartRace -= HideTrackSelect;
        EventManager.onStartVR -= EnterVRMode;
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    //UI Logic toogled by events
    void EnterVRMode()
    {
        gameModeSelect.SetActive(false);
        vrAdiviseText.enabled = true;
        StartCoroutine(LoadVRMenuStateScene());
    }

    IEnumerator LoadVRMenuStateScene()
    {
        yield return new WaitForSeconds(5f);
        dummyCamera.SetActive(false);
        vrAdiviseText.enabled = false;
        GameManager.Instance.ChangeState(State.SelectTrackVR);
        yield return new WaitForSeconds(1f);

    }

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

    public void HideTrackSelect()
    {
        trackSelectNonVR.SetActive(false);
        dummyCamera.SetActive(false);
    }

    //UI Logic by Input
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.currentState == State.MainScreen)
            PlayGame();

        if (GameManager.Instance.gameLayerStatus == GAMESTATUS.PAUSED)
        {
            dummyCamera.SetActive(true);
            PauseMenu(true);
            print("HERE");
        }
        else if(GameManager.Instance.gameLayerStatus == GAMESTATUS.RUNNING)
        {

            dummyCamera.SetActive(false);
            PauseMenu(false);
        }
    }

    public void PlayGame()
    {
        EventManager.StartGame();
    }

    public void PauseMenu(bool toogle)
    {
        pauseMenu.SetActive(toogle);
        dummyCamera.SetActive(toogle);
        dummyCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
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


    //Stage select on Normal Cam UI
    public void FarmRoad()
    {
        GameManager.Instance.ChangeState(State.MooMooFarm_race);
    }

    public void UnderwaterRoad()
    {
        //if is unlocked
        GameManager.Instance.ChangeState(State.Underwater_race);
    }

    public void RainbowRoad()
    {
        //if is unlocked
        GameManager.Instance.ChangeState(State.RainbowRoad_race);
    }
}

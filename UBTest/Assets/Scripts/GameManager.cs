using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Patterns.Creational.Singleton;
using UnityEngine.Events;

[System.Serializable]public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { };

public class GameManager : Singleton<GameManager>
{
    //Current level
    //load unload lvl
    //Track game state
    //generate other persistent systems
    //PREGAME, RUNNING, PAUSED
    
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

    public GameObject[] SystemPrefabs;
    public EventGameState OnGameStateChanged;

    private List<GameObject> _instancedSystemPrefabs;
    private static GameManager instance;
    GameState _currentGameState = GameState.PREGAME;
    List<AsyncOperation> _loadOperations;

    private string _currentLevelName = string.Empty;

    public GameState currentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        _loadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefabs();
        
        //LoadLevel("Main");   
    }

    private void Update()
    {
        if (currentGameState != GameState.PREGAME)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }



    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            if(_loadOperations.Count == 0)
            {
                UpdateState(GameState.RUNNING);
            }
            
            //message transition between scenes

        }

        Debug.Log("Load complete");
    }

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("Unload complete");
    }

    void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.PREGAME: 
                Time.timeScale = 1.0f;
                break;
            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;
            default:
                break; 
        }

        OnGameStateChanged.Invoke(_currentGameState, previousGameState);

        //dispatch message
        //transition between scenes

    }

    //GameObject prefabInstance;
    void InstantiateSystemPrefabs()
    {
        for (int i = 0; i < SystemPrefabs.Length; ++i){
            Instantiate(SystemPrefabs[i]);
            //_instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);

        if(ao == null)
        {
            Debug.LogError("GameManager unable to load level");
            return;
        }

        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao); 
        _currentLevelName = levelName;
    }

    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        ao.completed += OnUnloadOperationComplete; ;

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        //for (int i = 0; i < _instancedSystemPrefabs.Count; ++i)
        //{
        //    Destroy(_instancedSystemPrefabs[i]);
        //}
        //_instancedSystemPrefabs.Clear();
    }

    public void StartGame()
    {
        LoadLevel("Main");
    }

    public void TogglePause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }
}
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Patterns.Creational.Singleton;
using System.Linq;

//public enum GameState
//{
//    INIT,
//    RUNNING,
//    PAUSED
//}

public class LevelManager //: Singleton<LevelManager>
{
    //What lvl the game currently is
    //load and unload current game lvls
    //keep track of game state
    //Generate other persistent systems

    [SerializeField] int m_lvl;
    [SerializeField] int m_currentLvl;
    List<AsyncOperation> asyncLevelList;
    //Singleton
    public static LevelManager _instance;

    private LevelManager()
    {
        asyncLevelList = new List<AsyncOperation>();

        if (_instance != null)
        {
            Debug.LogError("[Singleton] Trying to instantitate a second instance of a singleton class." + "Instance name: " + _instance);
        }
        else
        {
            _instance = this;
        }
    }

    void OnLoadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("Level Loaded");
        asyncLevelList.Add(ao);
    }

    void OnUnloadOperationIsComplete(AsyncOperation ao)
    {
        Debug.Log("Level Unloaded");
        if (asyncLevelList.Contains(ao))
            asyncLevelList.Remove(ao);
    }

    void LoadLevel(int level)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
        if(ao == null)
        {
            Debug.LogWarning("[LM] was unable to load the lvl: " + level);
            return;
        }

        //ao.completed += (ao) => { asyncLevelList.Add(ao); };
        ao.completed += OnLoadOperationComplete;
    }

    void UnloadLevel(int level)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(level, UnloadSceneOptions.None);
        if (ao == null)
        {
            Debug.LogWarning("[LM] was unable to unload the lvl: " + level);
            return;
        }

        ao.completed += OnUnloadOperationIsComplete;
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadLevel(m_lvl);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnloadLevel(m_lvl);
        }
    }


    public static bool isGamePaused => Mathf.Abs(Time.deltaTime) < float.Epsilon;
}

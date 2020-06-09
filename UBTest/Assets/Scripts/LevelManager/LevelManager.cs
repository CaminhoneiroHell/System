using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Patterns.Creational.Singleton;
<<<<<<< HEAD
using System.Linq;
using System;

//public enum GameState
//{
//    INIT,
//    RUNNING,
//    PAUSED
//}

=======

>>>>>>> 9c1f89c728882f12d78308d1661c572e8e112e51
public class LevelManager : Singleton<LevelManager>
{
    //What lvl the game currently is
    //load and unload current game lvls
    //keep track of game state
    //Generate other persistent systems

    private List<AsyncOperation> asyncLevelList;
    [SerializeField] int m_lvl;
<<<<<<< HEAD
    [SerializeField] int m_currentLvl;
    List<AsyncOperation> asyncLevelList;

    //Singleton
    //public static LevelManager _instance;

    //private LevelManager()
    //{
    //    asyncLevelList = new List<AsyncOperation>();

    //    if (_instance != null)
    //    {
    //        Debug.LogError("[Singleton] Trying to instantitate a second instance of a singleton class." + "Instance name: " + _instance);
    //    }
    //    else
    //    {
    //        _instance = this;
    //    }
    //}
=======
    [SerializeField][Header("Don't touch, dont work")] private int m_currentLvl;

    private void Start()
    {
        m_currentLvl = 0;
        asyncLevelList = new List<AsyncOperation>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadLevel(m_lvl);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnloadCurrentLevel();
        }
    }
>>>>>>> 9c1f89c728882f12d78308d1661c572e8e112e51

    void LoadLevel(int level)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogWarning("[LM] was unable to load the lvl: " + level);
            return;
        }

        //ao.completed += (ao) => { asyncLevelList.Add(ao); };
        ao.completed += OnLoadOperationComplete;
        m_currentLvl = level;
    }

    void UnloadCurrentLevel()
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(m_currentLvl, UnloadSceneOptions.None);
        if (ao == null)
        {
            Debug.LogWarning("[LM] was unable to unload the lvl: " + m_currentLvl);
            return;
        }

        ao.completed += OnUnloadOperationIsComplete;
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


    public static bool isGamePaused => Mathf.Abs(Time.deltaTime) < float.Epsilon;
}

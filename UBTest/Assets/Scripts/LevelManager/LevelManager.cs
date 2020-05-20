using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Patterns.Creational.Singleton;

public class LevelManager : Singleton<LevelManager>
{
    //What lvl the game currently is
    //load and unload current game lvls
    //keep track of game state
    //Generate other persistent systems

    private List<AsyncOperation> asyncLevelList;
    [SerializeField] int m_lvl;
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

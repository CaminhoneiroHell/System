
namespace CaminhoneiroHell.RedRoom.Strategy
{
    using System;
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Patterns.Creational.Singleton;
    public enum State
    {
        Init,
        MainMenu,
        Stage1,
        Final
    }

    public interface IFSMGameState
    {
        IEnumerator Enter();

        /// <summary>
        /// Destroy this gameobject here
        /// </summary>
        /// <returns></returns>
        IEnumerator Exit();
    }

    public class InitGameState : MonoBehaviour, IFSMGameState
    {
        public IEnumerator Enter()
        {
            yield return new WaitForEndOfFrame();
        }

        public IEnumerator Exit()
        {
            Destroy(this);
            print("Destroying");
            yield return new WaitForEndOfFrame();
        }

        void Update()
        {
            print("Updating Init State.");
        }
    }

    public class MainMenuGameState : MonoBehaviour, IFSMGameState
    {
        public IEnumerator Enter()
        {
            yield return new WaitForEndOfFrame();
        }

        public IEnumerator Exit()
        {
            Destroy(this);
            print("Destroying");
            yield return new WaitForEndOfFrame();
        }

        void Update()
        {
            print("Updating MenuGame State.");
        }
    }

    public interface IGameStatemanager
    {
        void ChangeState(State newState);

    }

    public class GameStateManager : Singleton<GameStateManager>, IGameStatemanager
    {
        public List<IFSMGameState> gameStatesList;
        public State _currentGameState { get; private set; }

        private InitGameState init;
        private MainMenuGameState mainMenu;

        void Start()
        {
            DontDestroyOnLoad(this);
            gameStatesList = new List<IFSMGameState>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ChangeState(State.Init);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                ChangeState(State.MainMenu);
            }
        }

        public void ChangeState(State newState)
        {
            if(gameStatesList.Count != 0){
                foreach(IFSMGameState igs in gameStatesList){
                    StartCoroutine(igs.Exit());
                }
                gameStatesList.Clear();
            }
            
            _currentGameState = newState;

            switch (_currentGameState)
            {
                case State.Init:

                    init = gameObject.AddComponent<InitGameState>();
                    gameStatesList.Add(init);
                    StartCoroutine(init.Enter());
                    break;

                case State.MainMenu:

                    mainMenu = gameObject.AddComponent<MainMenuGameState>();
                    gameStatesList.Add(mainMenu);
                    StartCoroutine(mainMenu.Enter());

                    break;
                case State.Stage1:
                    break;
                case State.Final:
                    break;
                default:
                    break;
            }
        }
    }
}


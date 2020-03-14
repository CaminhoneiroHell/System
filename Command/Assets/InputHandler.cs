using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public GameObject actor;
    Animator anim;
    Command keyQ, keyW, keyE, upArrow;
    List<Command> oldCommands = new List<Command>();

    Coroutine replayCoroutine;
    bool shouldStartReplay;
    bool isReplaying;

    // Start is called before the first frame update
    void Start()
    {
        keyQ = new PerformJump();
        keyW = new PerformKick();
        keyE = new PerformPunch();
        upArrow = new MoveForward();
        anim = actor.GetComponent<Animator>();
        Camera.main.GetComponent<CameraFollow360>().player = actor.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReplaying)
            HandleInput();
        StartReplay();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            keyQ.Execute(anim, true);
            oldCommands.Add(keyQ);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            keyW.Execute(anim, true);
            oldCommands.Add(keyW);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            keyE.Execute(anim, true);
            oldCommands.Add(keyE);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            upArrow.Execute(anim, true);
            oldCommands.Add(upArrow);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            shouldStartReplay = true;

        if (Input.GetKeyDown(KeyCode.Z))
            UndoLastCommand();

    }

    void UndoLastCommand()
    {
        if (oldCommands.Count > 0)
        {
            Command c = oldCommands[oldCommands.Count - 1];
            c.Execute(anim, false);
            oldCommands.RemoveAt(oldCommands.Count - 1);
        }
    }

    void StartReplay()
    {
        if(shouldStartReplay && oldCommands.Count > 0)
        {
            shouldStartReplay = false;
            if(replayCoroutine != null)
            {
                StopCoroutine(replayCoroutine);
            }
            replayCoroutine = StartCoroutine(ReplayCommands());
        }
    }

    IEnumerator ReplayCommands()
    {
        isReplaying = true;

        for(int i = 0; i < oldCommands.Count; i++)
        {
            oldCommands[i].Execute(anim, true);
            yield return new WaitForSeconds(1f);
        }

        isReplaying = false;
    }
}

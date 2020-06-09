using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Patterns.Creational.Singleton;

public class CommandManager : Singleton<CommandManager>
{
    private List<ICommand> _commandBuffer = new List<ICommand>();

    public void AddCommandOnBuffer(ICommand command)
    {
        _commandBuffer.Add(command);
        print(_commandBuffer.Count);
    }

    public void PlayRoutineTrigger() {
        StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine(){
        foreach(ICommand cmd in _commandBuffer){
            cmd.Execute();
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void RewindRoutineTrigger(){
        StartCoroutine(RewindRoutine());
    }

    IEnumerator RewindRoutine(){
        foreach(ICommand cmd in Enumerable.Reverse(_commandBuffer)) {
            cmd.Undue();
            yield return new WaitForSeconds(1.0f);
        }
    }

    //Done = Finished with all colors. Turn them all white
    public void Done()
    {
        foreach (ICommand cmd in Enumerable.Reverse(_commandBuffer))
        {
            cmd.Undue();
        }
    }

    //Reset : Clear command buffer
    public void ClearBuffer(){
        _commandBuffer.Clear();
    }

}

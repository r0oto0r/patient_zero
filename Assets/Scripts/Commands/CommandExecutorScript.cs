using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandExecutorScript : MonoBehaviour
{
    public int CommandCount = 0;
    private List<Command> commandList = new List<Command>();
    void Start() {
        
    }

    void Update() {

    }

    public void AddCommand(Command command) {
        this.commandList.Add(command);
        Debug.Log(command.GetCommandText());
        command.Execute();
    }

    public void ExecuteCommands() {
        foreach(Command command in this.commandList) {
            Debug.Log(command.GetCommandText());
            command.Execute();
        }
    }
}

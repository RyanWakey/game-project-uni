using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor : MonoBehaviour
{
    private List<Command> commands = new List<Command>();
    private int currentCommandIndex = -1;
    public void ExecuteCommand(Command _command)
    {
        commands.Add(_command);
        _command.Execute();

        currentCommandIndex = commands.Count - 1;

    }
    public void UndoCommand()
    {
        if (currentCommandIndex < 0) return;

        commands[currentCommandIndex].Undo();
        commands.RemoveAt(currentCommandIndex);
        currentCommandIndex--;
    }
}

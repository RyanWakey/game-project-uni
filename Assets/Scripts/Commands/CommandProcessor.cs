using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor : MonoBehaviour
{
    private List<Command> commands = new List<Command>();
    private int currentCommandIndex = -1;

    public void RecordCommand(Command _command)
    {
        commands.Add(_command);
        currentCommandIndex = commands.Count - 1;
    }

    public void ExecuteCommand(Command _command)
    {
        commands.Add(_command);
        _command.Execute();

        currentCommandIndex = commands.Count - 1;

    }
    public void UndoCommand()
    {
        if (currentCommandIndex < 0) return;
        // Start undoing from the last command
        int undoIndex = currentCommandIndex;

        // Check if the last two commands are simultaneous
        if (undoIndex > 0)
        {
            bool isLastThrust = commands[undoIndex] is ThrustCommand;
            bool isBeforeLastThrust = commands[undoIndex - 1] is ThrustCommand;

            bool isLastRotation = commands[undoIndex] is MoveLeftCommand || commands[undoIndex] is MoveRightCommand;
            bool isBeforeLastRotation = commands[undoIndex - 1] is MoveLeftCommand || commands[undoIndex - 1] is MoveRightCommand;

            if ((isLastThrust && isBeforeLastRotation) || (isLastRotation && isBeforeLastThrust))
            {
                // Undo both commands
                commands[undoIndex].Undo();
                commands.RemoveAt(undoIndex);
                currentCommandIndex--;

                undoIndex--;

                commands[undoIndex].Undo();
                commands.RemoveAt(undoIndex);
                currentCommandIndex--;

                return;
            }
        }

        // If not simultaneous, undo the last command only
        commands[undoIndex].Undo();
        commands.RemoveAt(undoIndex);
        currentCommandIndex--;
    }
}

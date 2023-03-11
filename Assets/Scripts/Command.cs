public abstract class Command
{
    //represents the thing were applyign the command to - can be any object / player / npc etc.
    // IEntity is something that a command can be applied to.
    protected IEntity entity;

    public Command(IEntity e)
    {
        entity = e; 
    }

    public abstract void Execute();
    public abstract void Undo();
}

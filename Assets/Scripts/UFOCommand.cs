using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOCommand : Command
{
    private Vector2 position;
    private Vector2 velocity;

    public UFOCommand(IEntity entity) : base(entity)
    {
        this.position = entity.rb2D.position;
        this.velocity = entity.rb2D.velocity;
    }

    public override void Execute()
    {
        entity.rb2D.position = position;
        entity.rb2D.velocity = velocity;
    }

    public override void Undo()
    {
        entity.rb2D.position = position;
        entity.rb2D.velocity = velocity;
    }
}

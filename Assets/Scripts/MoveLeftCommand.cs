using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftCommand : Command
{
    private float rotationSpeed;
    public MoveLeftCommand(IEntity entity, float rotationSpeed) : base(entity)
    {
        this.rotationSpeed = rotationSpeed;
    }

    public override void Execute()
    {
        entity.rb2D.rotation -= rotationSpeed * Time.deltaTime;
    }

    public override void Undo()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightCommand : Command
{
    private float rotationSpeed;
    public MoveRightCommand(IEntity entity, float rotationSpeed) : base(entity)
    {
        this.rotationSpeed = rotationSpeed;
    }

    public override void Execute()
    {
        entity.rb2D.rotation += rotationSpeed * Time.deltaTime;
    }

    public override void Undo()
    {

    }
}

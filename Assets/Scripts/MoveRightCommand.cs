using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightCommand : Command
{
    private float rotationSpeed;
    private float rotationPerformed;
    public MoveRightCommand(IEntity entity, float rotationSpeed) : base(entity)
    {
        this.rotationSpeed = rotationSpeed;
    }

    public override void Execute()
    {
        rotationPerformed = rotationSpeed * Time.deltaTime;
        entity.rb2D.rotation -= rotationPerformed;
    }

    public override void Undo()
    {
        entity.rb2D.rotation += rotationPerformed;
    }
}

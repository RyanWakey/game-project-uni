using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftCommand : Command
{
    private float rotationSpeed;
    private float rotationPerformed;
    public MoveLeftCommand(IEntity entity, float rotationSpeed) : base(entity)
    {
        this.rotationSpeed = rotationSpeed;
    }

    public override void Execute()
    {
        rotationPerformed = rotationSpeed * Time.deltaTime;
        entity.rb2D.rotation += rotationPerformed;
    }

    public override void Undo()
    {
        entity.rb2D.rotation -= rotationPerformed;
    }
}

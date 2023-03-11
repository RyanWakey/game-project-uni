using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustCommand : Command
{
    private float engineForce;
    public ThrustCommand(IEntity entity, float engineForce) : base(entity)
    {
        this.engineForce = engineForce;
    }

    public override void Execute()
    {
        Vector2 newForce = engineForce * entity.tr.up;
        entity.rb2D.AddForce(newForce);
    }

    public override void Undo()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOMoveCommand : MonoBehaviour
{
    private Vector3 previousPosition;
    private Vector3 direction;
    private float speedFactor;

    public AsteroidMoveCommand(IEntity entity, Vector3 direction, float speedFactor) : base(entity)
    {
        this.previousPosition = entity.tr.position;
        this.direction = direction;
        this.speedFactor = speedFactor;
    }

    public override void Execute()
    {
        entity.rb2D.AddForce(direction * speedFactor);
    }

    public override void Undo()
    {
        entity.rb2D.position = previousPosition;
        entity.rb2D.velocity = Vector2.zero;
    }
}

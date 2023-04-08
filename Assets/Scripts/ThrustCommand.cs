using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustCommand : Command
{
    private float engineForce;
    private List<(Vector2 position, Vector2 velocity)> positionVelocityPairs;

    public ThrustCommand(IEntity entity, float engineForce) : base(entity)
    {
        this.engineForce = engineForce;
        this.positionVelocityPairs = new List<(Vector2 position, Vector2 velocity)>();
    }

    public override void Execute()
    { 
        Vector2 newForce = engineForce * entity.tr.up;
        positionVelocityPairs.Add((entity.tr.position, entity.rb2D.velocity));
        entity.rb2D.AddForce(newForce);
    }

    public override void Undo()
    {
        if (positionVelocityPairs.Count > 0)
        {
            (Vector2 lastPosition, Vector2 lastVelocity) = positionVelocityPairs[positionVelocityPairs.Count - 1];
            CoroutineRunner.Instance.StartCoroutine(Rewind(lastPosition, lastVelocity));
            positionVelocityPairs.RemoveAt(positionVelocityPairs.Count - 1);
        }
    }

    private IEnumerator Rewind(Vector2 targetPosition, Vector2 targetVelocity)
    {
        float duration = 0.35f;
        float elapsedTime = 0f;
        Vector2 initialPosition = entity.tr.position;
        Vector2 initialVelocity = entity.rb2D.velocity;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            entity.tr.position = Vector2.Lerp(initialPosition, targetPosition, t);
            entity.rb2D.velocity = Vector2.Lerp(initialVelocity, targetVelocity, t);

            yield return null;
        }

        entity.tr.position = targetPosition;
        entity.rb2D.velocity = targetVelocity;
    }
}


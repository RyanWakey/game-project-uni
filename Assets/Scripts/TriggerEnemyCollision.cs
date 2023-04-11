using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEnemyCollision : MonoBehaviour
{
    [SerializeField] private PlayerMovementController player;

    public UnityEvent<bool> OnTriggerChange;
    public UnityEvent<AsteroidController> OnCollisionEnter;
    public UnityEvent<AsteroidController> OnCollisionExit;

    private AsteroidController colliding;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.IsInvulnerable)
        {
            return;
        }

        OnTriggerChange?.Invoke(true);
        colliding = collision.gameObject.GetComponentInParent<AsteroidController>();
        if (colliding) OnCollisionEnter?.Invoke(colliding); 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerChange?.Invoke(false);
        if (colliding) OnCollisionExit?.Invoke(colliding);
    }
}

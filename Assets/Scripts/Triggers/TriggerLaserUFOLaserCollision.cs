using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerLaserUFOLaserCollision : MonoBehaviour
{
    [SerializeField] private PlayerMovementController player;

    public UnityEvent<bool> OnTriggerChange;
    public UnityEvent<PlayerMovementController> OnCollisionEnter;
    public UnityEvent<PlayerMovementController> OnCollisionExit;

    private PlayerMovementController colliding;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.IsInvulnerable)
        {
            return;
        }

        LaserBeam laser = GetComponentInParent<LaserBeam>();
        if (laser != null && laser.laserType == LaserBeam.LaserType.UFOlaser)
        {
            OnTriggerChange?.Invoke(true);
            colliding = collision.gameObject.GetComponentInParent<PlayerMovementController>();
            if (colliding) OnCollisionEnter?.Invoke(colliding);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerChange?.Invoke(false);
        if (colliding) OnCollisionExit?.Invoke(colliding);
    }
}

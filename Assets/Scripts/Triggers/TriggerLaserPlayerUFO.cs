using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerLaserPlayerUFO : MonoBehaviour
{
    public UnityEvent<bool> OnTriggerChange;
    public UnityEvent<UFOController> OnCollisionEnter;
    public UnityEvent<UFOController> OnCollisionExit;

    private UFOController colliding;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LaserBeam laser = GetComponentInParent<LaserBeam>();
        if (laser != null && laser.laserType == LaserBeam.LaserType.PlayerLaser)
        {
            OnTriggerChange?.Invoke(true);
            colliding = collision.gameObject.GetComponentInParent<UFOController>();
            if (colliding) OnCollisionEnter?.Invoke(colliding);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerChange?.Invoke(false);
        if (colliding) OnCollisionExit?.Invoke(colliding);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerUFOCollision : MonoBehaviour
{
    public UnityEvent<bool> OnTriggerChange;
    public UnityEvent<UFOController> OnCollisionEnter;
    public UnityEvent<UFOController> OnCollisionExit;

    private UFOController colliding;

    private void OnTriggerEnter2D(Collider2D collision)
    {
          OnTriggerChange?.Invoke(true);
          colliding = collision.gameObject.GetComponentInParent<UFOController>();
          if (colliding) OnCollisionEnter?.Invoke(colliding);
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerChange?.Invoke(false);
        if (colliding) OnCollisionExit?.Invoke(colliding);
    }
}

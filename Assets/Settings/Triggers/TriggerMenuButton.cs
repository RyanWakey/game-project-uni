using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerMenuButton : MonoBehaviour
{
    [SerializeField] private UnityEvent onTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LaserBeam laser = collision.GetComponentInParent<LaserBeam>();
        if (laser != null)
        {
            onTrigger?.Invoke();
            Destroy(laser.gameObject);
        }
    }
}
  


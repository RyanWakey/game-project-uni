using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerMenuButton : MonoBehaviour
{
    [SerializeField] private UnityEvent onTrigger;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        LaserBeam laser = collision.GetComponent<LaserBeam>();
        onTrigger?.Invoke();
        Destroy(collision.gameObject);

    }

}
  


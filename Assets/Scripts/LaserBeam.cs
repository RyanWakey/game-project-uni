using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private float bulletLifeTime = 5.0f;
    private Rigidbody2D rb2d;
    private float speed = 1000f;
    
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    public void Laser(Vector3 direction)
    {
        rb2d.AddForce(direction * speed);
        Destroy(this.gameObject, bulletLifeTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private float speed = 1250f;
    
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    public void Laser(Vector3 direction)
    {
        rb2d.AddForce(direction * speed);
    }
}

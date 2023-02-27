using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rd2d;
    float speed = 5f;
    
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
}

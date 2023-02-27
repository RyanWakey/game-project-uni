using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]


public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float engineForce;
    private LaserBeam Laser;

    private Rigidbody2D rb2D;
    private Transform tr;
    private Vector2 newForce;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        tr = transform;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb2D.rotation -= rotationSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb2D.rotation += rotationSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            newForce = engineForce * tr.up;
            rb2D.AddForce(newForce);
        }
    }
    
    private void Fire()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]


public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float engineForce;
    [SerializeField] private LaserBeam laserBeam;

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

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
        
    }
    
    private void Fire() {
        LaserBeam _laser = Instantiate(laserBeam, tr.position, tr.rotation);
        _laser.Laser(tr.up);
    }
}

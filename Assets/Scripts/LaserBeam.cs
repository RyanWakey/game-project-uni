using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private float bulletLifeTime = 5.0f;
    
    private Rigidbody2D rb2d;
    private float speed = 1000f;
    private bool inAsteroid = false;
    private List<AsteroidController> asteroids = new List<AsteroidController>();

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    public void Laser(Vector3 direction)
    {
        rb2d.AddForce(direction * speed);
        Destroy(this.gameObject, bulletLifeTime);
    }

    private void Update()
    {
        Debug.Log(inAsteroid);
        if (inAsteroid)
        {
            foreach (var item in asteroids)
            {
                Destroy(item.gameObject);
                Destroy(this.gameObject);
            }
        }    
    
    }

    public void InAsteroidChange(bool _inAsteroid)
    {
        inAsteroid = _inAsteroid;
    }

    public void CollidingAsteroid(AsteroidController colliding)
    {
        if (!asteroids.Contains(colliding)) asteroids.Add(colliding);
    }

    public void CollidedAsteroid(AsteroidController colliding)
    {
        if (asteroids.Contains(colliding)) asteroids.Remove(colliding);
    }
}

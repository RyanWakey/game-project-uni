using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private float bulletLifeTime = 5.0f;
    [SerializeField] private ScreenWrapper screenWrapper;

    private Rigidbody2D rb2d;
    private Transform tr;
    private float speed = 1000f;
    private bool inAsteroid = false;
    private List<AsteroidController> asteroids = new List<AsteroidController>();

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        screenWrapper = FindObjectOfType<ScreenWrapper>();
        tr = transform;
    }
    public void Laser(Vector3 direction)
    {
        rb2d.AddForce(direction * speed);
        Destroy(this.gameObject, bulletLifeTime);
    }

    private void Update()
    {
        if (inAsteroid)
        {
            List<AsteroidController> asteroidsToDestroy = new List<AsteroidController>();
            foreach (var item in asteroids)
            {
                asteroidsToDestroy.Add(item);
            }
            
            foreach (var item in asteroidsToDestroy)
            {
                item.spawningAsteroids();
                Destroy(this.gameObject);
            } 
        }    
    }

    private void FixedUpdate()
    {
        screenWrapper.WrapAround(this.tr);
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

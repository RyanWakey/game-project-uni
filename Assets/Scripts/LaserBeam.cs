using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{   
    [SerializeField] private float bulletLifeTime = 0.5f;
    [SerializeField] private float speed = 1000f;
    public LaserType laserType;
    private ScreenWrapperController screenWrapper;
    private Rigidbody2D rb2d;
    private Transform tr;
    private SpriteRenderer spriteRenderer;

    private bool inAsteroid = false;
    private List<AsteroidController> asteroids = new List<AsteroidController>();

    private bool inPlayer = false;
    private List<PlayerMovementController> player = new List<PlayerMovementController>();

    private bool inUFO = false;
    private List<UFOController> ufos = new List<UFOController>();

    public enum LaserType
    {
        PlayerLaser = 0,
        UFOlaser = 1
    }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        screenWrapper = FindObjectOfType<ScreenWrapperController>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        tr = transform;
    }

    public void Laser(Vector3 direction, Color laserColour)
    {
        rb2d.AddForce(direction * speed);
        spriteRenderer.color = laserColour;
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
                GameManager.instance.AsteroidDestroyted(item);
            } 
        }

        if (inPlayer)
        {
            List<PlayerMovementController> playersToDestroy = new List<PlayerMovementController>();
            foreach (var item in player)
            {
                playersToDestroy.Add(item);
            }

            foreach (var item in playersToDestroy)
            {
                item.playerHasCollided();
                Destroy(this.gameObject);
            }
        }

        if (inUFO)
        {
            List<UFOController> ufosToDestroy = new List<UFOController>();
            foreach (var item in ufos)
            {
                ufosToDestroy.Add(item);
            }

            foreach (var item in ufosToDestroy)
            {
               GameManager.instance.UFODestroyed(item);
               Destroy(item.gameObject); 
               Destroy(this.gameObject);
               
                
            }
        }

    }

    private void FixedUpdate()
    {
        screenWrapper.WrapAround(this.tr, this.rb2d);
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

    public void InPlayerChange(bool _inPlayer)
    {
        inPlayer = _inPlayer;
    }

    public void ColldingPlayer(PlayerMovementController colliding)
    {
        if (!player.Contains(colliding)) player.Add(colliding);
    }

    public void CollidedPlayer(PlayerMovementController colliding)
    {
        if (player.Contains(colliding)) player.Remove(colliding);
    }

    public void InUFOChange(bool _inUFO)
    {
        inUFO = _inUFO;
    }

    public void ColldingUFO(UFOController colliding)
    {
        if (!ufos.Contains(colliding)) ufos.Add(colliding);
    }

    public void CollidedUFO(UFOController colliding)
    {
        if (ufos.Contains(colliding)) ufos.Remove(colliding);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(CommandProcessor))]

public class PlayerMovementController : MonoBehaviour, IEntity
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float engineForce;
    [SerializeField] private LaserBeam laserBeam;
    [SerializeField] private const KeyCode thrustKeyCode = KeyCode.W;

    private Rigidbody2D rb2D;
    private Transform tr;
    private Vector2 newForce;

    private CommandProcessor commandProcessor;
    private Command buttonThrust;

    private bool inAsteroid = false;
    private List<AsteroidController> asteroids = new List<AsteroidController>();
    private AsteroidController asteroid;

    Rigidbody2D IEntity.rb2D => rb2D;
    Transform IEntity.tr => tr;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        tr = transform;
        commandProcessor = GetComponent<CommandProcessor>();
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
    public void FixedUpdate()
    {
        if (inAsteroid)
        {
            foreach (var item in asteroids)
            {
                if (item.transform.localScale.x > 6.0f)
                {
                    asteroid = item;
                    
                    for(int j = 0; j < 2; j++) { 
                        asteroid.CreateAsteroidsOnDestruction();
                    }
                }
                Destroy(item.gameObject);
                Destroy(this.gameObject);
            } 
        }
    }

    private void Fire() {
        LaserBeam _laser = Instantiate(laserBeam, tr.position, tr.rotation);
        _laser.Laser(tr.up);
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

    public void AssignCommand(Button button, Command command)
    {
        if (button == Input.GetKey(thrustKeyCode))
        {
            buttonThrust = command;
        }
    }
}

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
    [SerializeField] private const KeyCode rotateLeftKeyCode = KeyCode.A;
    [SerializeField] private const KeyCode rotateRightKeyCode = KeyCode.D;
    [SerializeField] private const KeyCode undoKey = KeyCode.U;

    private Rigidbody2D rb2D;
    private Transform tr;

    private CommandProcessor commandProcessor;
    private Command buttonThrust;
    private Command buttonRotateLeft;
    private Command buttonRotateRight;


    private bool inAsteroid = false;
    private List<AsteroidController> asteroids = new List<AsteroidController>();

    Rigidbody2D IEntity.rb2D => rb2D;
    Transform IEntity.tr => tr;
    
    
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        commandProcessor = GetComponent<CommandProcessor>();
        tr = transform;
    }

    public void Update()
    {
        if (Input.GetKey(thrustKeyCode))
        {
            commandProcessor.ExecuteCommand(new ThrustCommand(this, engineForce));
        }
    
        if (Input.GetKey(rotateLeftKeyCode))
        {
            commandProcessor.ExecuteCommand(new MoveLeftCommand(this, rotationSpeed));
        }

        if (Input.GetKey(rotateRightKeyCode))
        {
            commandProcessor.ExecuteCommand(new MoveRightCommand(this, rotationSpeed));
        }

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if (Input.GetKey(undoKey))
        {
            commandProcessor.UndoCommand();
        }
    }
        
    
    public void FixedUpdate()
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
        else if (button == Input.GetKey(rotateLeftKeyCode))
        {
            buttonRotateLeft = command;
        }
        else if (button == Input.GetKey(rotateRightKeyCode))
        {
            buttonRotateRight = command;
        }
    }
}

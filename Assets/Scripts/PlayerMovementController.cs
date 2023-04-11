using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CommandProcessor))]

public class PlayerMovementController : MonoBehaviour, IEntity
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float engineForce;
    [SerializeField] private LaserBeam laserBeam;
    [SerializeField] private const KeyCode thrustKeyCode = KeyCode.W;
    [SerializeField] private const KeyCode rotateLeftKeyCode = KeyCode.A;
    [SerializeField] private const KeyCode rotateRightKeyCode = KeyCode.D;
    [SerializeField] private const KeyCode undoKey = KeyCode.U;
    [SerializeField] private ScreenWrapperController screenWrapper;
    [SerializeField] private int normalSortingOrder = 0;
    [SerializeField] private int invulnerableSortingOrder = 1;
    [SerializeField] private float invulnerableDuration = 2.0f;
    private float invulnerabilityTimer;
    public bool IsInvulnerable { get { return invulnerabilityTimer > 0; } }

    private Rigidbody2D rb2D;
    private Transform tr;
    private SpriteRenderer spriteRenderer;

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
        screenWrapper = FindObjectOfType<ScreenWrapperController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        tr = transform;
    }

    public void Update()
    {
        if(invulnerabilityTimer > 0)
        {
            invulnerabilityTimer -= Time.deltaTime;
        }

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
        screenWrapper.WrapAround(this.tr, this.rb2D);
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
                this.gameObject.SetActive(false);
               
                
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
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0.0f;
        GameManager.instance.playerDestroyed();
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

    public void EnableInvulnerability()
    {
        invulnerabilityTimer = invulnerableDuration;
    }
}

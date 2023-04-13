using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CommandProcessor))]

public class PlayerMovementController : MonoBehaviour, IEntity
{
    [SerializeField] private GameObject childWithPolyCollider;
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
    [SerializeField] private float shootingtimerCD = 0.1f;
    [SerializeField] private Sprite destroyedShipSprite;
    [SerializeField] private ParticleSystem thrustParticleEffect;
    [SerializeField] private AudioClip thurstSoundEffect;
    private AudioSource thrustSource;

    private float invulnerabilityTimer;
    public bool IsInvulnerable { get { return invulnerabilityTimer > 0; } }

    private Rigidbody2D rb2D;
    private Transform tr;
    private SpriteRenderer spriteRenderer;
    private Sprite defaultShip;
    private PolygonCollider2D polygonCollider2D;

    private CommandProcessor commandProcessor;
    private Command buttonThrust;
    private Command buttonRotateLeft;
    private Command buttonRotateRight;

    private bool canShoot = true;
    private bool isDead = false;

    private bool inAsteroid = false;
    private List<AsteroidController> asteroids = new List<AsteroidController>();

    private bool inUFO = false;
    private List<UFOController> ufos = new List<UFOController>();

    Rigidbody2D IEntity.rb2D => rb2D;
    Transform IEntity.tr => tr;
    
    
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        commandProcessor = GetComponent<CommandProcessor>();    
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider2D = childWithPolyCollider.GetComponentInChildren<PolygonCollider2D>();  
        thrustSource = gameObject.AddComponent<AudioSource>();
        thrustSource.volume = 0.5f;
        tr = transform;
    }

    private void Start()
    {
        defaultShip = spriteRenderer.sprite;
    }

    public void Update()
    {
        if (isDead == false)
        {
            if (invulnerabilityTimer > 0)
            {
                invulnerabilityTimer -= Time.deltaTime;
                if (invulnerabilityTimer <= 0)
                {
                    spriteRenderer.sortingOrder = normalSortingOrder;
                }
            }

            if (Input.GetKey(thrustKeyCode))
            {
                commandProcessor.ExecuteCommand(new ThrustCommand(this, engineForce));
                if (!thrustSource.isPlaying)
                {
                    thrustSource.clip = thurstSoundEffect;
                    thrustSource.Play();
                }
                if (!thrustParticleEffect.isPlaying)
                {
                    thrustParticleEffect.Play();
                }

            }  else
            {
                if (thrustSource.isPlaying)
                {
                    thrustSource.Stop();
                }
                if (thrustParticleEffect.isPlaying)
                {
                    thrustParticleEffect.Stop();
                }
            }

                if (Input.GetKey(rotateLeftKeyCode))
            {
                commandProcessor.ExecuteCommand(new MoveLeftCommand(this, rotationSpeed));
            }

            if (Input.GetKey(rotateRightKeyCode))
            {
                commandProcessor.ExecuteCommand(new MoveRightCommand(this, rotationSpeed));
            }

            if (Input.GetMouseButtonDown(0) && canShoot)
            {
                Fire();
                StartCoroutine(shootingCD());
            }

            if (Input.GetKey(undoKey))
            {
                commandProcessor.UndoCommand();
            }
        }
    }
        
        
    
    public void FixedUpdate()
    {
        screenWrapper.WrapAround(this.tr, this.rb2D);
        if (inAsteroid)
        {
            List <AsteroidController> asteroidsToDestroy = new List<AsteroidController>();

            foreach (var item in asteroids)
            {
                asteroidsToDestroy.Add(item);
            }

            foreach (var item in asteroidsToDestroy)
            {
                item.spawningAsteroids();
                GameManager.instance.AsteroidDestroyted(item);
                spriteRenderer.sprite = destroyedShipSprite;
                polygonCollider2D.enabled = false;
                StartCoroutine(RemovePlayerAfterDeathTimer(1f));
                
                   
            }
        }

        if (inUFO)
        {
            List<UFOController> UFOsToDestroy = new List<UFOController>();

            foreach (var item in ufos)
            {
                UFOsToDestroy.Add(item);
            }

            foreach (var item in UFOsToDestroy)
            {
                Destroy(item);
            }
        }

    }

    private void Fire() {
        LaserBeam _laser = Instantiate(laserBeam, tr.position, tr.rotation);
        _laser.laserType = LaserBeam.LaserType.PlayerLaser;
        _laser.Laser(tr.up, Color.blue);
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
        isDead = true;
    }

    public void CollidedAsteroid(AsteroidController colliding)
    {
        if (asteroids.Contains(colliding)) asteroids.Remove(colliding);
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
        spriteRenderer.sortingOrder = invulnerableSortingOrder;
    }

    private IEnumerator shootingCD()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootingtimerCD);
        canShoot = true;
    }

    private IEnumerator RemovePlayerAfterDeathTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
        spriteRenderer.sprite = defaultShip;
        polygonCollider2D.enabled = true;
        isDead = false;
    }
}

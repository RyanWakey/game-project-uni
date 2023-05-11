using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CommandProcessor))]

public class PlayerMovementController : MonoBehaviour, IEntity
{
    [SerializeField] private GameObject childWithPolyCollider;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float engineForce;
    [SerializeField] private LaserBeam laserBeam;
    [SerializeField] private KeyCode undoKey = KeyCode.U;
    [SerializeField] private ScreenWrapperController screenWrapper;
    [SerializeField] private int normalSortingOrder = 0;
    [SerializeField] private int invulnerableSortingOrder = 3;
    [SerializeField] private float invulnerableDuration = 2.0f;
    [SerializeField] private float shootingtimerCD = 0.2f;
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

    public KeyCode thrustKeyCode;
    public KeyCode rotateLeftKeyCode;
    public KeyCode rotateRightKeyCode;
    public KeyCode fireKey;

    private bool canShoot = true;
    private bool isDead = false;

    private bool inAsteroid = false;
    private List<AsteroidController> asteroids = new List<AsteroidController>();

    private bool inUFO = false;
    private List<UFOController> ufos = new List<UFOController>();

    Rigidbody2D IEntity.rb2D => rb2D;
    Transform IEntity.tr => tr;

    private float aliveTime;
    private int asteroidKillCount;


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
        UpdateKeysFromPlayerPrefs();
    }

    public void Update()
    {
        if (isDead == false)
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                aliveTime += Time.deltaTime;
                
                if (aliveTime >= 30f && !AchievementManager.instance.GetAchievement(Achievement.AchievementType.StayAliveFor30SecondsWithoutDieing).isUnlocked)
                {
                    AchievementManager.instance.NotifyAchievementComplete(Achievement.AchievementType.StayAliveFor30SecondsWithoutDieing);
                    aliveTime = 0;
                    //Debug.Log("Stay Alive");
                }
            }

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

            if (Input.GetKey(fireKey) && canShoot)
            {
                StartCoroutine(ShootingCD());
            }

            if (Input.GetKey(undoKey))
            {
                commandProcessor.UndoCommand();
            }
        } else
        {
            aliveTime = 0;
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
                item.SpawningAsteroids();
                GameManager.instance.AsteroidDestroyted(item);
                playerHasCollided();
                AsteroidDestroyed();
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
                item.DestroyUfo(item.gameObject);
                GameManager.instance.AsteroidSound();
                playerHasCollided();
            }
        }

    }

    private void Fire() {
        LaserBeam _laser = Instantiate(laserBeam, tr.position, tr.rotation);
        _laser.laserType = LaserBeam.LaserType.PlayerLaser;
        GameManager.instance.LaserSound();
        _laser.Laser(tr.up, Color.blue);
    }

    public void AsteroidDestroyed()
    {
        asteroidKillCount++;
        if (asteroidKillCount == 1)
        {
            StartCoroutine(ResetKillCountAfterDelay(5f));
        }

        if (asteroidKillCount >= 10 && !AchievementManager.instance.GetAchievement(Achievement.AchievementType.Kill10AsteroidsIn5Seconds).isUnlocked)
        {
            AchievementManager.instance.NotifyAchievementComplete(Achievement.AchievementType.Kill10AsteroidsIn5Seconds);
            asteroidKillCount = 0; 
        }
    }

    private IEnumerator ResetKillCountAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        asteroidKillCount = 0;
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

    public void InUFOChange(bool _inUFO)
    {
        inUFO = _inUFO;
    }

    public void CollidingUFO(UFOController colliding)
    {
        if (!ufos.Contains(colliding)) ufos.Add(colliding);
    }

    public void CollidedUFO(UFOController colliding)
    {
        if (ufos.Contains(colliding)) ufos.Remove(colliding);
    }

    private IEnumerator ShootingCD()
    {
        Fire();
        canShoot = false;
        yield return new WaitForSeconds(shootingtimerCD);
        canShoot = true;
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

    public void playerHasCollided()
    {
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0.0f;
        GameManager.instance.PlayerDestroyed();
        isDead = true;
        spriteRenderer.sprite = destroyedShipSprite;
        polygonCollider2D.enabled = false;
        StartCoroutine(RemovePlayerAfterDeathTimer(1f));
    }

    private IEnumerator RemovePlayerAfterDeathTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
        spriteRenderer.sprite = defaultShip;
        polygonCollider2D.enabled = true;
        isDead = false;
    }

    public void UpdateKeysFromPlayerPrefs()
    {
        int profileIndex = ProfileManager.instance.GetProfileIndex();
        string prefix = "Profile" + profileIndex + ",";

        thrustKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(prefix + "Thrust", "W"));
        rotateLeftKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(prefix + "RotateLeft", "A"));
        rotateRightKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(prefix + "RotateRight", "D"));
        fireKey = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(prefix + "Fire", "Mouse0"));
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class AsteroidController : MonoBehaviour, IEntity
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float asteroidLifeTime = 80.0f;
    [SerializeField] ScreenWrapperController screenWrapper;

    public static List<AsteroidController> asteroids = new List<AsteroidController>();
    public CommandProcessor commandProcessor { get; set; }


    private SpriteRenderer sprititeRenderer;
    private Rigidbody2D rb2D;
    private Transform tr;

    public float size { get; set; }

    private float minAsteroidSize = 12f;
    private Vector2 asteroidSizeRange = new Vector2(25f, 30f);
    private float speed = 1000.0f;
    public Vector2 initialVelocity { get; set; }

    Rigidbody2D IEntity.rb2D => rb2D;
    Transform IEntity.tr => tr;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        sprititeRenderer = rb2D.GetComponent<SpriteRenderer>();
        screenWrapper = FindObjectOfType<ScreenWrapperController>();
        tr = transform;
        size = Random.Range(asteroidSizeRange.x, asteroidSizeRange.y);
        commandProcessor = GetComponent<CommandProcessor>();
    }

    private void Start()
    {
        sprititeRenderer.sprite = sprites[Random.Range(0,sprites.Length)];
        tr.localScale = Vector3.one * this.size;
        tr.rotation = Quaternion.Euler(0.0f, 0.0f, Random.value * 360.0f);
        asteroids.Add(this);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.U))
        {
            commandProcessor.UndoCommand();
        }
        else
        {
            commandProcessor.RecordCommand(new AsteroidMoveCommand(this));
        }

    }


    public void FixedUpdate()
    {
        screenWrapper.WrapAround(this.tr, this.rb2D);
    }

    public void SetTrajectory(Vector3 direction)
    {
        float speedFactor = 15.0f / this.size;
        rb2D.AddForce(direction * speed * speedFactor);
        initialVelocity = rb2D.velocity;
        Destroy(this.gameObject, asteroidLifeTime);
    }

    public void CreateAsteroidsOnDestruction()
    {
        Vector2 position = tr.position;
        position += Random.insideUnitCircle * 10.0f;

        AsteroidController asteroidSmaller = Instantiate(this, position, tr.rotation);
        asteroidSmaller.size = size * 0.5f;
        asteroidSmaller.SetTrajectory(Random.insideUnitCircle.normalized);

    }

    public void SpawningAsteroids()
    {
        if (size > minAsteroidSize)
        {
            for(int i = 0; i < 2; i++)
            {
                CreateAsteroidsOnDestruction();
            }
        }
        GameManager.instance.AsteroidSound();
        DestroyAsteroid(this.gameObject);
       
    }

    void DestroyAsteroid(GameObject asteroid)
    {
        PointsManaging scoringObject = asteroid.GetComponent<PointsManaging>();
        if (scoringObject != null)
        {
            int points = scoringObject.PointValue;
            GameManager.instance.AddScore(points);
        }
        asteroids.Remove(this);
        Destroy(this.gameObject);
    }

}

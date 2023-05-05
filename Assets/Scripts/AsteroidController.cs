using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class AsteroidController : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float asteroidLifeTime = 80.0f;
    [SerializeField] ScreenWrapperController screenWrapper;


    private SpriteRenderer sprititeRenderer;
    private Rigidbody2D rb2d;
    private Transform tr;

    private float size;
    private float minAsteroidSize = 12f;
    private Vector2 asteroidSizeRange = new Vector2(25f, 30f);
    private float speed = 1000.0f;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprititeRenderer = rb2d.GetComponent<SpriteRenderer>();
        screenWrapper = FindObjectOfType<ScreenWrapperController>();
        tr = transform;
        size = Random.Range(asteroidSizeRange.x, asteroidSizeRange.y);

    }

    private void Start()
    {
        sprititeRenderer.sprite = sprites[Random.Range(0,sprites.Length)];
        tr.localScale = Vector3.one * this.size;
        tr.rotation = Quaternion.Euler(0.0f, 0.0f, Random.value * 360.0f);
    }

    public void FixedUpdate()
    {
        screenWrapper.WrapAround(this.tr, this.rb2d);
    }

    public void SetTrajectory(Vector3 direction)
    {
        float speedFactor = 15.0f / this.size;
        rb2d.AddForce(direction * speed * speedFactor);
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
        Destroy(this.gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class AsteroidController : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float asteroidLifeTime = 120.0f;
    [SerializeField] private ScreenWrapper screenWrapper;

    private SpriteRenderer sprititeRenderer;
    private Rigidbody2D rb2d;
    private Transform tr;
   

    private int asteroidPhase = 1;
    private float size = 30f;
    private float speed = 1000.0f;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprititeRenderer = rb2d.GetComponent<SpriteRenderer>();
        tr = transform;
        screenWrapper = FindObjectOfType<ScreenWrapper>();
    }

    private void Start()
    {
        sprititeRenderer.sprite = sprites[Random.Range(0,sprites.Length)];
        tr.localScale = Vector3.one * this.size;
        tr.rotation = Quaternion.Euler(0.0f, 0.0f, Random.value * 360.0f);
    }

    public void FixedUpdate()
    {
        screenWrapper.WrapAround(this.tr);
    }

    public void SetTrajectory(Vector3 direction)
    {
        float speedFactor = 15.0f / this.size;
        rb2d.AddForce(direction * speed * speedFactor);
        Destroy(this.gameObject, asteroidLifeTime);
    }

    public void CreateAsteroidsOnDestruction(int newPhase)
    {
        Vector2 position = tr.position;
        position += Random.insideUnitCircle * 10.0f;

        AsteroidController asteroidSmaller = Instantiate(this, position, tr.rotation);
        asteroidSmaller.size = size * 0.5f;
        asteroidSmaller.asteroidPhase += asteroidPhase + 1;
        asteroidSmaller.SetTrajectory(Random.insideUnitCircle.normalized);

    }

    public void spawningAsteroids()
    {
        if (asteroidPhase <= 3)
        {
            int newPhase = asteroidPhase + 1;
            for(int i = 0; i < 2; i++)
            {
                CreateAsteroidsOnDestruction(newPhase);
            }
        }
        Destroy(this.gameObject);
    }  
}

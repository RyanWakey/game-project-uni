using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class AsteroidController : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float asteroidLifeTime = 120.0f;
    
    private SpriteRenderer sprititeRenderer;
    private Rigidbody2D rd2d;
    private float size = 8f;
    private float speed = 20.0f;
    private Transform tr;

    private void Awake()
    {
        rd2d = GetComponent<Rigidbody2D>();
        sprititeRenderer = rd2d.GetComponent<SpriteRenderer>();
        tr = transform;

    }
    private void Start()
    {
        sprititeRenderer.sprite = sprites[Random.Range(0,sprites.Length)];
        tr.localScale = Vector3.one * size;
        tr.rotation = Quaternion.Euler(0.0f, 0.0f, Random.value * 360.0f);
    }
    public void SetTrajectory(Vector3 direction)
    {
        rd2d.AddForce(direction * speed);
        Destroy(this.gameObject, asteroidLifeTime);
    }

    public void CreateAsteroidsOnDestruction()
    {
        Vector2 position = tr.position;
        position += Random.insideUnitCircle * 0.5f;

        AsteroidController asteroidSmaller = Instantiate(this, position, tr.rotation);
        asteroidSmaller.size = size * 0.5f;
        asteroidSmaller.SetTrajectory(Random.insideUnitCircle.normalized * 2.0f);
    }
}

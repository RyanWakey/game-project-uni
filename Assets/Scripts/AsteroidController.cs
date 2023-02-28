using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    private SpriteRenderer sprititeRenderer;

    private Rigidbody2D rd2d;

    private BoxCollider2D bc2D;

    private float size = 1.2f;
    private float speed = 500f;
    private Transform tr;
    private void Awake()
    {
        rd2d = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        sprititeRenderer = rd2d.GetComponent<SpriteRenderer>();
        tr = transform;

    }
    private void Start()
    {
        sprititeRenderer.sprite = sprites[Random.Range(0,sprites.Length)];

        tr.localScale = Vector3.one * size;
   
    }
    public void SetTrajectory(Vector3 direction)
    {
        rd2d.AddForce(speed * direction);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsterioidController : MonoBehaviour
{
    public Sprite[] sprites;

    private SpriteRenderer sprititeRenderer;

    private Rigidbody2D rd2d;

    private PolygonCollider2D pc2D;

    private float size = 0.8f;
    private float speed = 2f;

    private void Awake()
    {
        rd2d = GetComponent<Rigidbody2D>();
        pc2D = GetComponent<PolygonCollider2D>();
        sprititeRenderer = rd2d.GetComponent<SpriteRenderer>();

    }
    private void Start()
    {
        sprititeRenderer.sprite = sprites[Random.Range(0,sprites.Length)];
   
    }
}

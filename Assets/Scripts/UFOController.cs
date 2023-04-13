using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LaserBeam laser;
    [SerializeField] private float shootingtimerCD = 9.0f;
    [SerializeField] ScreenWrapperController screenWrapper;
    [SerializeField] private float UFOLifeTime = 70.0f;


    private Vector2 UFOSizeRange = new Vector2(15f, 20f);
    private Rigidbody2D rb2d;
    private Transform tr;
   

    private bool canShoot = true;
    private float size;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        screenWrapper = FindObjectOfType<ScreenWrapperController>();
        tr = transform;
        size = Random.Range(UFOSizeRange.x, UFOSizeRange.y);
    }

    private void Start()
    {
        tr.localScale = Vector3.one * this.size;
    }

    private void Update()
    {
        if (canShoot)
        {
            StartCoroutine(shootingCD());
        }
    }

    public void FixedUpdate()
    {
        screenWrapper.WrapAround(this.tr, this.rb2d);
    }

    private void Fire()
    {
        Vector2 randomDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        LaserBeam _laser = Instantiate(laser, tr.position, tr.rotation);
        _laser.laserType = LaserBeam.LaserType.UFOlaser;
        _laser.Laser(randomDirection, Color.red);
    }

    private IEnumerator shootingCD()
    {
        Fire();
        canShoot = false;
        yield return new WaitForSeconds(shootingtimerCD);
        canShoot = true;
    }

    public void SetTrajectory(Vector3 direction)
    {
        float speedFactor = 10.0f / this.size;
        rb2d.AddForce(direction * speed * speedFactor);
        Destroy(this.gameObject, UFOLifeTime);
    }
}

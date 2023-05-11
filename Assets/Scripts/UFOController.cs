using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : MonoBehaviour, IEntity
{
    [SerializeField] private float speed;
    [SerializeField] private LaserBeam laser;
    [SerializeField] private float shootingtimerCD = 9.0f;
    [SerializeField] ScreenWrapperController screenWrapper;
    [SerializeField] private float UFOLifeTime = 70.0f;


    private Vector2 UFOSizeRange = new Vector2(15f, 20f);
    private Rigidbody2D rb2D;
    private Transform tr;

    private CommandProcessor commandProcessor;

    private bool canShoot = true;
    private float size;

    Rigidbody2D IEntity.rb2D => rb2D;
    Transform IEntity.tr => tr;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        screenWrapper = FindObjectOfType<ScreenWrapperController>();
        tr = transform;
        size = Random.Range(UFOSizeRange.x, UFOSizeRange.y);
        commandProcessor = GetComponent<CommandProcessor>();
    }

    private void Start()
    {
        tr.localScale = Vector3.one * this.size;
    }

    private void Update()
    {
        if (canShoot)
        {
            StartCoroutine(ShootingCD());
        }

        if (Input.GetKey(KeyCode.U))
        {
            commandProcessor.UndoCommand();
        }
        else
        {
            commandProcessor.RecordCommand(new UFOCommand(this));
        }
        
    }

    public void FixedUpdate()
    {
        screenWrapper.WrapAround(this.tr, this.rb2D);
    }

    private void Fire()
    {
        Vector2 randomDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        LaserBeam _laser = Instantiate(laser, tr.position, tr.rotation);
        _laser.laserType = LaserBeam.LaserType.UFOlaser;
        _laser.Laser(randomDirection, Color.red);
    }

    private IEnumerator ShootingCD()
    {
        Fire();
        GameManager.instance.LaserSound();
        canShoot = false;
        yield return new WaitForSeconds(shootingtimerCD);
        canShoot = true;
    }

    public void SetTrajectory(Vector3 direction)
    {
        float speedFactor = 10.0f / this.size;
        rb2D.AddForce(direction * speed * speedFactor);
        Destroy(this.gameObject, UFOLifeTime);
    }

    public void DestroyUfo(GameObject asteroid)
    {
        PointsManaging scoringObject = asteroid.GetComponent<PointsManaging>();
        if (scoringObject != null)
        {
            int points = scoringObject.PointValue;
            GameManager.instance.AddScore(points);
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private AsteroidController asteroid;
    [SerializeField] private float spawnRate = 4.0f;
    [SerializeField] private float spawnDistance = 15f;
    [SerializeField] private float enemeyAngleVariance = 20.0f;
    // will need ufo to

    private Transform tr;
   
 

    private void Start()
    {
        InvokeRepeating(nameof(Spawner), 0f, spawnRate);  
    }

    // Update is called once per frame
    private void Spawner()
    {
        Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
        Vector3 spawnPoint = this.transform.position + spawnDirection;

        float angleVariance = Random.Range(-enemeyAngleVariance, enemeyAngleVariance);
        Quaternion rotation = Quaternion.AngleAxis(angleVariance, Vector3.forward);

        AsteroidController asteroid = Instantiate(this.asteroid, spawnPoint, rotation);
        asteroid.SetTrajectory(rotation * -spawnDirection);
    }

   
}

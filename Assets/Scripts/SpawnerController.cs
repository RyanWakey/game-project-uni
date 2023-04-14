using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private AsteroidController asteroid;
    [SerializeField] private UFOController UFO;
    [SerializeField] private float asteroidSpawnRate;
    [SerializeField] private float UFOSpawnRate;
    [SerializeField] private float spawnDistance;
    [SerializeField] private float enemeyAngleVariance = 10.0f;


    private Transform tr;
   
 

    private void Start()
    {
        InvokeRepeating("AsteroidSpawner", 0f, asteroidSpawnRate);
        InvokeRepeating("UFOSpawner", 15f, UFOSpawnRate);
    }


    private void AsteroidSpawner()
    {
        Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
        Vector3 spawnPoint = this.transform.position + spawnDirection;

        float angleVariance = Random.Range(-enemeyAngleVariance, enemeyAngleVariance);
        Quaternion rotation = Quaternion.AngleAxis(angleVariance, Vector3.forward);

        AsteroidController asteroid = Instantiate(this.asteroid, spawnPoint, rotation);
        asteroid.SetTrajectory(-spawnDirection.normalized);
    }

    private void UFOSpawner()
    {
        Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
        Vector3 spawnPoint = this.transform.position + spawnDirection;

        UFOController UFO = Instantiate(this.UFO, spawnPoint, Quaternion.identity);
        UFO.SetTrajectory(-spawnDirection.normalized);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private AsteroidController asteroid;
    [SerializeField] private float spawnRate = 0.1f;
    [SerializeField] private float spawnDistance = 150.0f;
    [SerializeField] private float enemeyAngleVariance = 10.0f;
    // will need ufo to

    private Transform tr;
   
 

    private void Start()
    {
        InvokeRepeating("Spawner", 0f, spawnRate);  
    }


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

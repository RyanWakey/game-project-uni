using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementController player;
    [SerializeField] private int lives = 3;
    [SerializeField] private ParticleSystem asteroidExplosion;
    [SerializeField] private Text livesText;

    private float respawnTime = 3.0f;
    public static GameManager instance {  get; private set; }

    public void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
        {
            instance = this;
        }
    }
    
    public void AsteroidDestroyted(AsteroidController asteroid)
    {
        this.asteroidExplosion.transform.position = asteroid.transform.position;
        this.asteroidExplosion.Play();
    }

    public void playerDestroyed()
    {

        this.lives--;
        UpdateLivesText();
        if(this.lives == 0)
        {
            //End
        } else
        {
            Invoke("RespawnPlayer", this.respawnTime);
        }

       
    }

    private void RespawnPlayer()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.SetActive(true);
        this.player.EnableInvulnerability();
    }

    private void UpdateLivesText()
    {
        livesText.text = "Lives: " + lives;
    }
}

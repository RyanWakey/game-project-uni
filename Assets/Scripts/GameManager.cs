using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementController playerPrefab;
    [SerializeField] private int lives = 3;
    [SerializeField] private ParticleSystem asteroidExplosion;
    [SerializeField] private TextMeshProUGUI livesText;

    private string sceneName;
    private PlayerMovementController player;
    public static bool loadPlayer = false;

    [SerializeField] private AudioClip laserSoundEffect;
    private AudioSource laserSource;

    [SerializeField] private AudioClip asteroidDestroyedSoundEffect;
    private AudioSource asteroidSource;

    private float respawnTime = 3.0f;
    public static GameManager instance {  get; private set; }

    public void Awake()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        laserSource = gameObject.AddComponent<AudioSource>();
        asteroidSource = gameObject.AddComponent<AudioSource>();
        player = FindAnyObjectByType<PlayerMovementController>();
        if (instance != null)
        { 
            Destroy(gameObject);
            return;
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }


    public void AsteroidDestroyted(AsteroidController asteroid)
    {
        this.asteroidExplosion.transform.position = asteroid.transform.position;
        this.asteroidExplosion.Play();
    }

    public void UFODestroyed(UFOController ufo)
    {
        this.asteroidExplosion.transform.position = ufo.transform.position;
        this.asteroidExplosion.Play();
        asteroidSound();
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

    public void laserSound()
    {
        laserSource.PlayOneShot(laserSoundEffect);
    }

    public void asteroidSound()
    {
        asteroidSource.PlayOneShot(asteroidDestroyedSoundEffect);
    }

   
    
    public void LoadScene(string sceneName)
    {
        if(player != null)
        {
            Destroy(player.gameObject);
        }
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            player = FindAnyObjectByType<PlayerMovementController>();
            livesText = GameObject.Find("LivesText").GetComponent<TextMeshProUGUI>();
            UpdateLivesText();
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
        }

    
}

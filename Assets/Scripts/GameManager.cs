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
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private string sceneName;
    private PlayerMovementController player;
    public static bool loadPlayer = false;

    [SerializeField] private AudioClip laserSoundEffect;
    private AudioSource laserSource;

    [SerializeField] private AudioClip asteroidDestroyedSoundEffect;
    private AudioSource asteroidSource;

    private float respawnTime = 3.0f;
    public static GameManager instance { get; private set; }
    public int score { get; private set; }
    public float timePlayed { get; private set; }
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
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    private void Update()
    {
        if (sceneName == "Game")
        {
            UpdateTimePlayed();
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
        AsteroidSound();
    }

    public void PlayerDestroyed()
    {
        this.lives--;
        UpdateLivesText();
        if (this.lives == 0)
        {
            ShowGameOverMessage();
            StartCoroutine(IncrementPlayCount());
        }
        else
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

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public void LaserSound()
    {
        laserSource.PlayOneShot(laserSoundEffect);
    }

    public void AsteroidSound()
    {
        asteroidSource.PlayOneShot(asteroidDestroyedSoundEffect);
    }

    public void LoadScene(string sceneName)
    {
        if (player != null)
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
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
            gameOverText = GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>();
            gameOverText.gameObject.SetActive(false);
            UpdateLivesText();
            UpdateScoreText();
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
        if (score > GetHighScore())
        {
            SaveHighScore(score);
        }
    }

    public void SaveHighScore(int score)
    {
        int profileIndex = ProfileManager.instance.GetProfileIndex();
        string prefix = "Profile" + profileIndex + ",";
        PlayerPrefs.SetInt(prefix + "HighScore", score);
    }

    public int GetHighScore()
    {
        int profileIndex = ProfileManager.instance.GetProfileIndex();
        string prefix = "Profile" + profileIndex + ",";
        return PlayerPrefs.GetInt(prefix + "HighScore", 0);
    }

    public int GetPlayCount()
    {
        int profileIndex = ProfileManager.instance.GetProfileIndex();
        string prefix = "Profile" + profileIndex + ",";
        return PlayerPrefs.GetInt(prefix + "PlayCount", 0);
    }

    public void SavePlayerProgress()
    {
        int profileIndex = ProfileManager.instance.GetProfileIndex();
        string prefix = "Profile" + profileIndex + ",";

        PlayerPrefs.SetInt(prefix + "HighScore", GetHighScore());
        float previousTimePlayed = PlayerPrefs.GetFloat(prefix + "TimePlayed", 0);
        float totalTimePlayed = previousTimePlayed + timePlayed;
        PlayerPrefs.SetFloat(prefix + "TimePlayed", totalTimePlayed);
        PlayerPrefs.SetInt(prefix + "GamesPlayed",GetPlayCount());
        PlayerPrefs.Save();

        Debug.Log("high score is :" + GetHighScore());
        Debug.Log("Current profile is: " + ProfileManager.instance.GetProfileIndex());
    }

    public IEnumerator IncrementPlayCount()
    {
        // laser can still hit an object after death
        yield return new WaitForSeconds(2f);

        int profileIndex = ProfileManager.instance.GetProfileIndex();
        string prefix = "Profile" + profileIndex + ",";
        int playCount = PlayerPrefs.GetInt(prefix + "PlayCount", 0);
        playCount++;
        PlayerPrefs.SetInt(prefix + "PlayCount", playCount);
        PlayerPrefs.Save();

        SavePlayerProgress();
    }

    private void UpdateTimePlayed()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            timePlayed += Time.deltaTime;
        }
    }


    public void ShowGameOverMessage()
    {
        gameOverText.gameObject.SetActive(true);
        StartCoroutine(FlashGameOverText());
    }

    private IEnumerator FlashGameOverText()
    {
        float flashDuration = 1f;
        float elapsedTime = 0f;

        while (true)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.PingPong(elapsedTime, flashDuration) / flashDuration;
            gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, alpha);
            yield return null;
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip laserSoundEffect;
    private AudioSource laserSource;

    [SerializeField] private AudioClip asteroidDestroyedSoundEffect;
    private AudioSource asteroidSource;
    public static SoundManager instance { get; private set; }
    public void Awake()
    {
        laserSource = gameObject.AddComponent<AudioSource>();
        asteroidSource = gameObject.AddComponent<AudioSource>();

        if (instance)
            Destroy(gameObject);
        else
        {
            instance = this;
        }
    }

    public void laserSound()
    {
        laserSource.PlayOneShot(laserSoundEffect);
    }

    public void asteroidSound()
    {
        asteroidSource.PlayOneShot(asteroidDestroyedSoundEffect);
    }

}

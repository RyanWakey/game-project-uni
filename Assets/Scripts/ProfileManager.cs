using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerProgress(int profileIndex)
    {
       // PlayerPrefs.SetInt("Profile" + profileIndex + "_Score", GameManager.instance.Score);
       // PlayerPrefs.SetFloat("Profile" + profileIndex + "_TimePlayed", GameManager.instance.TimePlayed);
        PlayerPrefs.Save();
    }

    public string LoadProfile(int profileIndex)
    {
        return PlayerPrefs.GetString("Profile" + profileIndex, "");
    }

    public bool ProfileExists(int profileIndex)
    {
        return PlayerPrefs.HasKey("Profile" + profileIndex);
    }
}

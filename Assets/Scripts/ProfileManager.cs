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

    public void SaveProfile(int profileIndex, string data)
    {
        PlayerPrefs.SetString("Profile" + profileIndex, data);
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

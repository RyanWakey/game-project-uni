using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance;
    private int currentProfileIndex = 0;

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

    public void SetProfileIndex(int profileIndex)
    {
        currentProfileIndex = profileIndex;
    }

    public int GetProfileIndex()
    {
        return currentProfileIndex;
    }

}

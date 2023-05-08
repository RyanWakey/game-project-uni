using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] highScoreTexts;
    [SerializeField] private TextMeshProUGUI[] timePlayedTexts;
    [SerializeField] private TextMeshProUGUI[] gamesPlayedTexts;
    [SerializeField] private TextMeshProUGUI[] achievementsUnlockedText;
    [SerializeField] private Color selectedProfile = Color.green;
    [SerializeField] private Color unselectedProfile = Color.white;

    private void Start()
    {
        LoadProfiles();
        UpdateProfileColors();
    }

    private void LoadProfiles()
    {
        for (int i = 0; i < 3; i++)
        {
           SetProfileData(i);
        }
    }

    public void SetProfileData(int profileIndex)
    {
        string prefix = "Profile" + profileIndex + ",";

        int highScore = PlayerPrefs.GetInt(prefix + "HighScore", 0);
        float timePlayed = PlayerPrefs.GetFloat(prefix + "TimePlayed", 0);
        int gamesPlayed = PlayerPrefs.GetInt(prefix + "GamesPlayed", 0);

        int timePlayedInMinutes = Mathf.FloorToInt(timePlayed / 60);

        highScoreTexts[profileIndex].text = "High Score: " + highScore;
        timePlayedTexts[profileIndex].text = "Time Played: " + timePlayedInMinutes + " mins";
        gamesPlayedTexts[profileIndex].text = "Games Played: " + gamesPlayed;


        UpdateAchievementsTexts();
        UpdateProfileColors();
    }


    public void ClearCurrentProfileData()
    {
        int currentProfileIndex = ProfileManager.instance.GetProfileIndex();
        string prefix = "Profile" + currentProfileIndex + ",";

        PlayerPrefs.SetInt(prefix + "HighScore", 0);
        PlayerPrefs.SetFloat(prefix + "TimePlayed", 0);
        PlayerPrefs.SetInt(prefix + "GamesPlayed", 0);
        AchievementManager.instance.ResetAchievementsForProfile(currentProfileIndex);
        PlayerPrefs.Save();

        UpdateProfileTextFields(currentProfileIndex);

        
    }
  
    public void UpdateProfileTextFields(int profileIndex)
    {
        string prefix = "Profile" + profileIndex + ",";

        int highScore = PlayerPrefs.GetInt(prefix + "HighScore", 0);
        float timePlayed = PlayerPrefs.GetFloat(prefix + "TimePlayed", 0);
        int gamesPlayed = PlayerPrefs.GetInt(prefix + "GamesPlayed", 0);

        highScoreTexts[profileIndex].text = "High Score: " + highScore;
        timePlayedTexts[profileIndex].text = "Time Played: " + timePlayed;
        gamesPlayedTexts[profileIndex].text = "Games Played: " + gamesPlayed;
        UpdateAchievementsTexts();
    }

    public void UpdateProfileColors()
    {
        int currentProfileIndex = ProfileManager.instance.GetProfileIndex();
        for (int i = 0; i < 3; i++)
        {
            if (i == currentProfileIndex)
            {
                highScoreTexts[i].color = selectedProfile;
                timePlayedTexts[i].color = selectedProfile;
                gamesPlayedTexts[i].color = selectedProfile;
                achievementsUnlockedText[i].color = selectedProfile;
            }
            else
            {
                highScoreTexts[i].color = unselectedProfile;
                timePlayedTexts[i].color = unselectedProfile;
                gamesPlayedTexts[i].color = unselectedProfile;
                achievementsUnlockedText[i].color = unselectedProfile;
            }
        }
    }

    public void UpdateAchievementsTexts()
    {
        int totalAchievements = Enum.GetValues(typeof(Achievement.AchievementType)).Length;

        for (int i = 0; i < 3; i++)
        {
            ProfileManager.instance.SetProfileIndex(i);
            int unlockedCount = AchievementManager.instance.GetUnlockedAchievementCount();
            achievementsUnlockedText[i].text = "Achievements Unlocked " + unlockedCount + "/" + totalAchievements;
        }

        ProfileManager.instance.SetProfileIndex(PlayerPrefs.GetInt("CurrentProfileIndex", 0));
    }


}

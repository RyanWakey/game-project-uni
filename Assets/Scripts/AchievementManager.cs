using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private Queue<Achievement.AchievementType> achievementQ = new Queue<Achievement.AchievementType>();
    private List<Achievement> achievements;

    public static AchievementManager instance { get; private set; }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void NotifyAchievementComplete(Achievement.AchievementType type)
    {
        achievementQ.Enqueue(type);
    }

    private void Start()
    {
        achievements = new List<Achievement>();
        LoadAchievements();
        StartCoroutine("AchievementQueueCheck");
    }

    private void UnlockAchievement(Achievement.AchievementType type)
    {
        Achievement curAchievement = null;

        foreach (Achievement achievement in achievements)
        {
            if(achievement.type == type)
            {
                curAchievement = achievement;
                break;
            }
        }
        if (curAchievement != null && !curAchievement.isUnlocked)
        {
            curAchievement.isUnlocked = true;
            int profileIndex = ProfileManager.instance.GetProfileIndex();
            string prefix = "Profile" + profileIndex + ",";
            PlayerPrefs.SetInt(prefix + "Achievement," + curAchievement.type, 1);
            PlayerPrefs.Save();
            Debug.Log("Achievement Unlocked: " + curAchievement.name);
        }
    }

    private IEnumerator AchievementQueueCheck()
    {
        for (;;)
        {
            if (achievementQ.Count > 0)
            {
                UnlockAchievement(achievementQ.Dequeue());
            }
            yield return new WaitForSeconds(5.0f);
        }
    }

    public void LoadAchievements()
    {
        int profileIndex = ProfileManager.instance.GetProfileIndex();
        string prefix = "Profile" + profileIndex + ",";

        foreach (Achievement.AchievementType type in Enum.GetValues(typeof(Achievement.AchievementType)))
        {
            string achievementName;
            string achievementDescription;

            switch (type)
            {
                case Achievement.AchievementType.StayAliveFor60SecondsWithoutDieing:
                    achievementName = "Dodger";
                    achievementDescription = "Stay alive for 60 seconds";
                    break;
                case Achievement.AchievementType.ReachScore1000:
                    achievementName = "Getting Better!";
                    achievementDescription = "Achieved a Score of 1000";
                    break;
                case Achievement.AchievementType.ReachScore5000:
                    achievementName = "Professional";
                    achievementDescription = "Achieved a Score of 5000";
                    break;
                case Achievement.AchievementType.ReachScore10000:
                    achievementName = "Elite";
                    achievementDescription = "Achieved a Score of 10000";
                    break;
                case Achievement.AchievementType.Kill10AsteroidsIn5Seconds:
                    achievementName = "KILL KILL KILL";
                    achievementDescription = "Kill 10 Asteroids in 5 seconds";
                    break;
                case Achievement.AchievementType.Reach10000scoreWithoutDieing:
                    achievementName = "GOD-LIKE";
                    achievementDescription = "Achieved a Score of 10000 without Dieing";
                    break;
                default:
                    achievementName = "";
                    achievementDescription = "";
                    break;
            }

            int unlockedValue = PlayerPrefs.GetInt(prefix + "Achievement," + type.ToString(), 0);
            bool isUnlocked;
            if (unlockedValue == 1)
            {
                isUnlocked = true;
            }
            else
            {
                isUnlocked = false;
            }

            achievements.Add(new Achievement(type, achievementName, achievementDescription, isUnlocked));
        }
    }
    public int GetUnlockedAchievementCount()
    {
        int count = 0;
        foreach (Achievement achievement in achievements)
        {
            if (achievement.isUnlocked)
            {
                count++;
            }
        }
        return count;
    }

    public void ResetAchievementsForProfile(int profileIndex)
    {
        string prefix = "Profile" + profileIndex + ",";

        foreach (Achievement.AchievementType type in Enum.GetValues(typeof(Achievement.AchievementType)))
        {
            PlayerPrefs.SetInt(prefix + "Achievement," + type.ToString(), 0);
        }

        PlayerPrefs.Save();

        if (ProfileManager.instance.GetProfileIndex() == profileIndex)
        {
            LoadAchievements();
        }
    }
}

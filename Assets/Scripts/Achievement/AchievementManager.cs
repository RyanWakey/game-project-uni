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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        achievements = new List<Achievement>();
        LoadAchievements();
    }

    public void NotifyAchievementComplete(Achievement.AchievementType type)
    {
        Achievement achievement = GetAchievement(type);
        if (achievement != null && !achievement.isUnlocked)
        {
            achievementQ.Enqueue(type);
        }

    }

    public Achievement GetAchievement(Achievement.AchievementType type)
    {
        foreach (Achievement achievement in achievements)
        {
            if (achievement.type == type && achievement.profileIndex == ProfileManager.instance.GetProfileIndex())
            {
                //Debug.Log("this has happend how many times then");
                return achievement;
            }
        }
        return null;
    }

    private void Start()
    {
        StartCoroutine("AchievementQueueCheck");
    }

    private IEnumerator AchievementQueueCheck()
    {
        while (true)
        {
            if (achievementQ.Count > 0)
            {
                //Debug.Log("UNLOCKING ACHIEVEMENT");
                UnlockAchievement(achievementQ.Dequeue());
            }
            yield return new WaitForSeconds(5.0f);
        }
    }

    private void UnlockAchievement(Achievement.AchievementType type)
    {
        Achievement curAchievement = GetAchievement(type);

        if (curAchievement != null && !curAchievement.isUnlocked)
        {
            curAchievement.isUnlocked = true;
            int profileIndex = ProfileManager.instance.GetProfileIndex();
            string prefix = "Profile" + profileIndex + ",";
            PlayerPrefs.SetInt(prefix + "Achievement," + curAchievement.type, 1);
            PlayerPrefs.Save();
            //Debug.Log("Achievement Unlocked: " + curAchievement.isUnlocked + " " + curAchievement.profileIndex + " " + curAchievement.name);
            AchievementNotification.instance.ShowNotification("Achievement Unlocked: " + curAchievement.name + "\n" + curAchievement.description);
        }
    }

    public void LoadAchievements()
    {
        achievementQ.Clear();
        for (int profileIndex = 0; profileIndex < 3; profileIndex++)
        {
            string prefix = "Profile" + profileIndex + ",";

            foreach (Achievement.AchievementType type in Enum.GetValues(typeof(Achievement.AchievementType)))
            {
                string achievementName;
                string achievementDescription;

                switch (type)
                {
                    case Achievement.AchievementType.StayAliveFor30SecondsWithoutDieing:
                        achievementName = "Dodger";
                        achievementDescription = "Stay alive for 30 seconds";
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

                int unlockedValue = PlayerPrefs.GetInt(prefix + "Achievement," + type, 0);
                bool isUnlocked = unlockedValue == 1;

                achievements.Add(new Achievement(type, achievementName, achievementDescription, isUnlocked, profileIndex));
            }
        }
    }


    public int GetUnlockedAchievementCount(int profileIndex)
    {
        int count = 0;
        foreach (Achievement achievement in achievements)
        {
            if (achievement.isUnlocked && achievement.profileIndex == profileIndex)
            {
                count++;
            }
        }
        return count;
    }


    public void ResetAchievementsForProfile(int profileIndex)
    {
        string prefix = "Profile" + profileIndex + ",";

        foreach (Achievement achievement in achievements)
        {
            if (achievement.profileIndex == profileIndex)
            {
                achievement.isUnlocked = false;
                PlayerPrefs.SetInt(prefix + "Achievement," + achievement.type.ToString(), 0);
            }
        }

        PlayerPrefs.Save();

        if (ProfileManager.instance.GetProfileIndex() == profileIndex)
        {
            LoadAchievements();
        }
    } 
}

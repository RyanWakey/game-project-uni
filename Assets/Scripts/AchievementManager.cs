using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivementManager : MonoBehaviour
{
    private Queue<Achievement> achivementQ = new Queue<Achievement>();
    private List<Achievement> achievements;


    public void NotifyAchievementComplete(Achievement.AchievementType type)
    {
        achivementQ.Enqueue(new Achievement(type));
    }

    private void Start()
    {
        StartCoroutine("AchievementQueueCheck");
    }

    private void UnlockAchievement(Achievement achievement)
    {
        if (!achievement.isUnlocked)
        {
            achievement.isUnlocked = true;
            Debug.Log($"Unlocked achievement: {achievement.name}");
        }
    }

    private IEnumerator AchievementQueueCheck()
    {
        for (; ; )
        {
            if (achivementQ.Count > 0)
            {
                Achievement achivementToUnlock = achivementQ.Dequeue();
                UnlockAchievement(achivementToUnlock);
                yield return new WaitForSeconds(5.0f);
            }
        }
    }

    private IEnumerator IncrementAsteroidCounter()
    {
        asteroidCounter++;

        if (asteroidCounter >= 10)
        {
            NotifyAchievementComplete(Achievement.AchievementType.Kill10AsteroidsIn5Seconds);
        }

        yield return new WaitForSeconds(5f);
        asteroidCounter--;
    }

    public void CheckAchievement(Achievement.AchievementType type)
    {
        Achievement achievement = achievements.Find(a => a.type == type);

        if (achievement != null)
        {
            NotifyAchievementComplete(type);
        }
    }
}

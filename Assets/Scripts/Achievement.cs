using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement : MonoBehaviour
{
   public enum AchievementType 
    { 
        StayAliveFor30Seconds,
        ReachScore1000,
        ReachScore5000,
        Kill10AsteroidsIn5Seconds
    } 

    public AchievementType Type;
    public string Name;
    public string Description;
    public float isUnlocked;

    public Achievement(AchievementType type, string name, string description, float isUnlocked)
    {
        Type = type;
        Name = name;
        Description = description;
        this.isUnlocked = isUnlocked;
    }
}

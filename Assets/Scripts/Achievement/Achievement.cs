using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement
{
    public enum AchievementType
    {
        StayAliveFor30SecondsWithoutDieing,
        ReachScore1000,
        ReachScore5000,
        ReachScore10000,
        Reach10000scoreWithoutDieing,
        Kill10AsteroidsIn5Seconds
    }

    public AchievementType type;
    public string name;
    public string description;
    public bool isUnlocked;
    public int profileIndex;

    public Achievement(AchievementType _type, string _name, string _description, bool _isUnlocked, int _profileIndex)
    {
        type = _type;
        name = _name;
        description = _description;
        isUnlocked = _isUnlocked;
        profileIndex = _profileIndex;
    }
}

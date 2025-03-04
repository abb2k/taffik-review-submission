using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public bool showCharInfo;
    public bool VisualEffects;
    public int HighScore;
    public int SelectedOffcie;
    public int TenthSecsIn5020Mode;

    public List<string> completedChallenges;

    public float GameVolume;

    public int frigidsCount;
    public int coinsCount;
    public int batteriesCount;
    public int DDRepelsCount;

    public bool DebugMode;
}

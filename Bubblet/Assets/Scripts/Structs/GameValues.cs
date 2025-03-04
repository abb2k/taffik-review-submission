using System;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct GameValues
{
    public int randomsCount;
    public int turrentsCount;
    public int treesCount;
    public int cactiCount;

    [Space]

    public int waveCount;
    public static GameValues operator +(GameValues a, GameValues b)
    {
        GameValues toReturn = new GameValues();

        toReturn.randomsCount = a.randomsCount + b.randomsCount;
        toReturn.turrentsCount = a.turrentsCount + b.turrentsCount;
        toReturn.treesCount = a.treesCount + b.treesCount;
        toReturn.cactiCount = a.cactiCount + b.cactiCount;

        toReturn.waveCount = a.waveCount + b.waveCount;

        return toReturn;
    }
}

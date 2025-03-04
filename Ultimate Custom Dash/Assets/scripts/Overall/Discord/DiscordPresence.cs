using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DiscordPresence
{
    public DiscordPresence(string _State, string _Details, int _TimestampsStart, int _TimestampsEnd, string _LargeImage, string _LargeText, string _SmallImage, string _SmallText)
    {
        State = _State;
        Details = _Details;
        LargeImage = _LargeImage;
        LargeText = _LargeText;
        SmallImage = _SmallImage;
        SmallText = _SmallText;
    }

    public string State;
    public string Details;
    [Space]
    public string LargeImage;
    public string LargeText;
    [Space(4)]
    public string SmallImage;
    public string SmallText;
}

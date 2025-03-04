using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class teamCredit
{
    public string Name;
    [ShowAssetPreview]
    public Sprite pfp;
    [Space]
    public socials Socials;
    [Space]
    public tags Tags;
    [Space]
    public string Message;

    [System.Serializable]
    public class socials
    {
        public string Youtube;
        public string Twitter;
        public string Twitch;
    }

    [System.Serializable]
    public class tags
    {
        public bool Dev;
        public bool BetaTester;
        public bool Helper;
        public bool GFX;
    }
}

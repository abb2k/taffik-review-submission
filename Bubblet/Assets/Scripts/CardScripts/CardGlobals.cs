using System;
using System.Collections.Generic;
using UnityEngine;

public static class CardGlobals
{
    struct CardDiffColor
    {
        public Color fill;
        public Color text;

        public CardDiffColor(Color fill, Color text)
        {
            this.fill = fill;
            this.text = text;
        }
    }

    static private Dictionary<cardDifficulty, CardDiffColor> diffColorList = new Dictionary<cardDifficulty, CardDiffColor>()
    {
        {
            cardDifficulty.Easy,
            new CardDiffColor(
                new Color32(58, 144, 0, 255),
                new Color32(51, 143, 61, 255)
            )
        },
        {
            cardDifficulty.Medium,
            new CardDiffColor(
                new Color32(161, 102, 0, 255),
                new Color32(130, 85, 0, 255)
            )
        },
        {
            cardDifficulty.Hard,
            new CardDiffColor(
                new Color32(161, 21, 0, 255),
                new Color32(125, 0, 32, 255)
            )
        }
    };

    public static void getColorsBasedOnDifficulty(cardDifficulty diff, out Color fillColor, out Color textColor)
    {
        if (!diffColorList.ContainsKey(diff))
        {
            fillColor = Color.white;
            textColor = Color.white;
            return;
        }

        fillColor = diffColorList[diff].fill;
        textColor = diffColorList[diff].text;
    }
}

public enum cardDifficulty
{
    Easy,
    Medium,
    Hard
}

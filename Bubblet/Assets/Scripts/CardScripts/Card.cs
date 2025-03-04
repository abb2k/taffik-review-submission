using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    public cardDifficulty difficulty;
    public float multiplierAddition;

    [Space]

    public Sprite icon;
    public string textIcon;
    public Material iconMaterial;
    public string title;
    [Multiline]
    public string description;

    [Space]

    public GameValues addedValues;
}
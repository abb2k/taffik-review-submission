using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class buffCard : MonoBehaviour, IMenuInteractable
{
    //adde values, multiplier added
    public event UnityAction<GameValues, float> onSelected;

    [SerializeField] private Image fill;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI textIcon;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI multiplierText;
    private GameValues vals;
    private float multiplier;

    public void onInteractEnter(player p)
    {
        //buff and continue stuff


        onSelected.Invoke(vals, multiplier);
    }

    public void onInteractExit(player p)
    {
        
    }

    public void setCard(Card cardTemplate)
    {
        CardGlobals.getColorsBasedOnDifficulty(cardTemplate.difficulty, out Color fillCol, out Color textCol);

        fill.color = fillCol;

        if (cardTemplate.icon != null)
        {
            icon.sprite = cardTemplate.icon;
            icon.material = cardTemplate.iconMaterial;
        }
        else
        {
            icon.enabled = false;
            textIcon.text = cardTemplate.textIcon;
            textIcon.material = cardTemplate.iconMaterial;
        }

        title.color = textCol;
        title.text = cardTemplate.title;

        description.color = textCol;
        description.text = cardTemplate.description;

        multiplierText.text = $"X{cardTemplate.multiplierAddition} MULT";

        vals = cardTemplate.addedValues;
        multiplier = cardTemplate.multiplierAddition;
    }
}

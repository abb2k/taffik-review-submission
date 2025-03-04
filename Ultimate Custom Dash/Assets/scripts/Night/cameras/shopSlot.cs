using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class shopSlot : MonoBehaviour
{
    public bool Enabled;
    public string itemName;
    public int Cost;
    public TextMeshProUGUI costText;
    public GameObject buyButton;
    bool canBuy = true;

    public void setEnabled(bool b)
    {
        Enabled = b;
        costText.gameObject.SetActive(b);
        buyButton.SetActive(b);
    }

    public void SetCost(int cost)
    {
        Cost = cost;
        costText.text = cost.ToString();
    }

    public void Buy()
    {
        if (canBuy)
        {
            NightManager nm = NightManager.inctance;

            if (nm.FazCoins >= Cost)
            {
                nm.addFazCoins(-Cost);

                nm.ItemBought(itemName);
                canBuy = false;
                buyButton.SetActive(false);
                SoundManager.getSoundManager().CreateSoundEffect("buyItem", SoundManager.getSoundManager().GetSoundFromList("buyItem"));
            }
        }
    }
}

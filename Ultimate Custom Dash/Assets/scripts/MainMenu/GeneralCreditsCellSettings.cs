using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneralCreditsCellSettings
{
    public string name;
    [Multiline]
    public string description;
    [Space]
    public List<CreditLink> links;

    [System.Serializable]
    public class CreditLink
    {
        public string link;
        public Sprite Icon;
    }
}

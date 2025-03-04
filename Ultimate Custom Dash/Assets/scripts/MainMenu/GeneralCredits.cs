using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCredits : MonoBehaviour
{
    public Transform parent;
    public GameObject GeneralCellPrefab;
    public List<GeneralCreditsCellSettings> credits;

    private void Start()
    {
        for (int i = 0; i < credits.Count; i++)
        {
            Instantiate(GeneralCellPrefab, parent).GetComponent<GeneralCreditsCell>().SetCell(credits[i]);
        }
    }
}

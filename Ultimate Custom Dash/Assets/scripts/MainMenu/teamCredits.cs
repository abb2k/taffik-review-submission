using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class teamCredits : MonoBehaviour
{
    public Transform container;
    public GameObject cellPrefab;
    public List<teamCredit> credits;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < credits.Count; i++)
        {
            teamCreditCell tempCellPrefab = Instantiate(cellPrefab, container).GetComponent<teamCreditCell>();

            tempCellPrefab.SetCell(credits[i]);
        }
    }
}

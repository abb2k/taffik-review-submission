using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowOneCamInctance : MonoBehaviour
{
    private static AllowOneCamInctance inctance;

    // Start is called before the first frame update
    void Start()
    {
        if (inctance == null)
        {
            inctance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

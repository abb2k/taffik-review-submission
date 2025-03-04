using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowOneInctance : MonoBehaviour
{
    private static AllowOneInctance inctance;

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

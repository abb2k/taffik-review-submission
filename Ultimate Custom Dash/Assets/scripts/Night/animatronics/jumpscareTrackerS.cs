using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpscareTrackerS : MonoBehaviour
{
    public Action Mycallback = null;
    public void callback()
    {
        if (Mycallback != null)
        {
            Mycallback();
        }
    }
}

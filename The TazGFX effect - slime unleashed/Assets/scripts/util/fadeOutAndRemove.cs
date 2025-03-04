using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeOutAndRemove : MonoBehaviour
{   
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var color = sr.color;
        color.a = Mathf.MoveTowards(color.a, 0, 0.2f * Time.deltaTime);
        sr.color = color;
        if (color.a <= 0)
        {
            Destroy(gameObject);
        }

    }
}

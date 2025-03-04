using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localAudioSource : MonoBehaviour
{
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (source == null)
        {
            Destroy(gameObject);
        }
    }
}

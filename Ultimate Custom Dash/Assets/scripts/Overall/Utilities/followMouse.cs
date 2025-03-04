using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followMouse : MonoBehaviour
{
    public Camera renderingCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!renderingCamera)
        {
            var pos = Input.mousePosition;
            pos.z = -renderingCamera.transform.position.z;
            transform.position = Camera.main.ScreenToWorldPoint(pos);
        }
        else
        {
            var pos = Input.mousePosition;
            pos.z = -renderingCamera.transform.position.z;
            transform.position = renderingCamera.ScreenToWorldPoint(pos);

        }
        
    }
}

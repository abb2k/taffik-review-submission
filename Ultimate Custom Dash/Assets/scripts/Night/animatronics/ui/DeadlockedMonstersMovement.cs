using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlockedMonstersMovement : MonoBehaviour
{
    public bool GoingRight;
    public float speed;
    public float speedMultiplier;
    public Vector2 minMaxPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GoingRight)
        {
            var pos = transform.localPosition;
            pos.x = Mathf.MoveTowards(pos.x, minMaxPosition.y, speed * Time.deltaTime * speedMultiplier);
            transform.localPosition = pos;

            if (pos.x >= minMaxPosition.y)
            {
                GoingRight = false;
                transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            }
        }
        else
        {
            var pos = transform.localPosition;
            pos.x = Mathf.MoveTowards(pos.x, minMaxPosition.x, speed * Time.deltaTime * speedMultiplier);
            transform.localPosition = pos;

            if (pos.x <= minMaxPosition.x)
            {
                GoingRight = true;
                transform.localScale = new Vector3(-0.25f, 0.25f, 0.25f);
            }
        }
    }
}

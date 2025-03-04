using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    [ReadOnly]public float Speed;
    public Vector2 MaxPositions;
    public bool MovemendEnabled;
    public float paralaxingAmount;
    public Transform paralax;
    public NightManager NM;

    private void Start()
    {
        paralax.position = (Vector2)transform.position / paralaxingAmount;
    }

    void Update()
    {
        if (MovemendEnabled && !NM.beingJumpscared && !NM.isDead)
        {
            transform.position += new Vector3(1, 0, 0) * Time.deltaTime * Speed;

            var pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, MaxPositions.x, MaxPositions.y);
            transform.position = pos;

            paralax.position = pos / paralaxingAmount;
        }
    }

    public void setSpeed(float speed)
    {
        Speed = speed;
    }
}

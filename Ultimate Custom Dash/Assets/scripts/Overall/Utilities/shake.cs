using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [BoxGroup("settings")]
    public bool getOriginFromPos;
    [BoxGroup("settings")]
    [DisableIf("getOriginFromPos")]
    public Vector3 origin;
    [BoxGroup("settings")]
    public float range;
    [BoxGroup("settings")]
    public float speed;
    [BoxGroup("settings")]
    public bool shakeEnabled;

    [BoxGroup("constrains")]
    public bool x, y, z = true;

    Coroutine c;

    // Start is called before the first frame update
    void Start()
    {
        if (getOriginFromPos)
            origin = transform.localPosition;
    }

    IEnumerator shakeLoop()
    {
        while (shakeEnabled)
        {
            var pos = origin;

            if (!x)
                pos.x += Random.Range(-range, range);

            if (!y)
                pos.y += Random.Range(-range, range);

            if (!z)
                pos.z += Random.Range(-range, range);

            transform.localPosition = pos;

            yield return new WaitForSeconds(speed);
        }

        transform.localPosition = origin;
    }

    public void startShake()
    {
        shakeEnabled = true;
        if (c != null) StopCoroutine(c);
        c = StartCoroutine(shakeLoop());
    }

    public void startShake(float _range, float _speed)
    {
        range = _range;
        speed = _speed;

        startShake();
    }

    public void stopShake()
    {
        if (shakeEnabled)
            shakeEnabled = false;
    }
}

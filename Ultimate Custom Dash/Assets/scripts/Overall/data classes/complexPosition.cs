using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class complexPosition
{
    public complexPosition(Vector3 _position, Vector3 _rotation, Vector3 _scale)
    {
        position = _position;
        rotation = _rotation;
        scale = _scale;
    }
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale = Vector3.one;
}

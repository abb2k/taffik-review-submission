using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class keyPosition
{
    public keyPosition(Transform _parent, Vector3 _position, Vector3 _rotation, bool _posCaught)
    {
        parent = _parent;
        position = _position;
        rotation = _rotation;
        posCaught = _posCaught;
    }
    public Transform parent;
    public Vector3 position;
    public Vector3 rotation;
    public bool posCaught;
    public basementMonster.eyeColors heldColor;
}

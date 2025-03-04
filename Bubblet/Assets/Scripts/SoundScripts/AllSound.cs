using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllSound", menuName = "Scriptable Objects/AllSound")]
public class AllSound : ScriptableObject
{
    public List<Sound> allSounds;
}

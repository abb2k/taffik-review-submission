using UnityEngine;

[System.Serializable]
public struct Keybind
{
    [Tooltip("The key to use, if set to null then use mouse key ID.")]
    public KeyCode key;
    public int mouseKeyID;
}

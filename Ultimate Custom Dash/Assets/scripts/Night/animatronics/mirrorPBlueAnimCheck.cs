using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirrorPBlueAnimCheck : MonoBehaviour
{
    public MirrorPortal mp;
    public void OnAnimEnded()
    {
        mp.AddACharacter();
        GetComponent<Animator>().SetBool("animPlaying", false);
    }
}

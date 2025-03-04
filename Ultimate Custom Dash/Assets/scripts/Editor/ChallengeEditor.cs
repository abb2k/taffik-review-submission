using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Challenge))]
public class ChallengeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Challenge chal = (Challenge)target;

        chal.challengeText.text = chal.challengeName;

        if (chal.challengeName == string.Empty)
        {
            chal.gameObject.name = "Challenge";
        }
        else
        {
            string myname = chal.challengeName + " (Challenge)";
            chal.gameObject.name = myname;
        }
    }
}

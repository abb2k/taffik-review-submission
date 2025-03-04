using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ductPoint : MonoBehaviour
{
    public bool HasAudioLure;
    public ductPoint[] back;
    public ductPoint[] forward;
    public bool leftEnd;
    public bool rightEnd;
    public bool isAudioLureClose(out ductPoint point)
    {
        point = null;
        bool isLureNear = false;
        if (HasAudioLure)
        {
            isLureNear = true;
            point = this;
        }
        else
        {
            for (int i = 0; i < back.Length; i++)
            {
                if (back[i].HasAudioLure)
                {
                    isLureNear = true;
                    point = back[i];
                }
            }
            for (int i = 0; i < forward.Length; i++)
            {
                if (forward[i].HasAudioLure)
                {
                    isLureNear = true;
                    point = forward[i];
                }
            }
        }

         

        return isLureNear;
    }
}

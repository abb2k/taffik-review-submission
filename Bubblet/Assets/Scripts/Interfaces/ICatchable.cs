using UnityEngine;

public interface ICatchable
{
    float getHealOnCosume();
    float getMultiOnConsume();
    void OnCatch();
    Transform getCapturedObject();
}

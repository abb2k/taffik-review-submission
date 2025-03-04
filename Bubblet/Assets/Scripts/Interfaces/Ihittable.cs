using UnityEngine;

public interface Ihittable
{
    void OnHit(DamageInfo info, Transform KBOrigin = null);
}

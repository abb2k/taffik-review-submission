using UnityEngine;

public class SplitBullet : Bullet
{
    [SerializeField] private int splitAmount;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private DamageInfo splittedInfo;

    protected override void OnHit(Ihittable hit)
    {
        base.OnHit(hit);

        splitToAmount();
        Destroy(gameObject);
    }

    void splitToAmount()
    {
        float extraOreiantation = Random.Range(0f, 360f);

        float angleOffset = 360 / splitAmount;

        for (int i = 0; i < splitAmount; i++)
        {
            var rads = (angleOffset * i + extraOreiantation) * Mathf.Deg2Rad;
            Vector2 vec = new Vector2(Mathf.Cos(rads), Mathf.Sin(rads));

            GameObject bulletToSpawn = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(vec), transform.parent);
            Bullet bulletScript = bulletToSpawn.GetComponent<Bullet>();
            bulletScript.setKBOrigin(bulletToSpawn.transform);
            bulletScript.setInfo(splittedInfo);
            bulletScript.send(vec, speed);
        }
    }

    protected override void onBorderCollision()
    {
        splitToAmount();
        base.onBorderCollision();
    }
}

using System.Threading.Tasks;
using UnityEngine;

public class TreeEnemy : EnemyBase
{
    [Header("Tree")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int shootAmount;

    protected override void OnShoot()
    {
        animator.SetTrigger("Attack");
        
    }
    private void Shoot()
    {
        float extraOreiantation = Random.Range(0f, 360f);
        if (wasCaught) return;

        float angleOffset = 360 / shootAmount;

        for (int i = 0; i < shootAmount; i++)
        {
            var rads = (angleOffset * i + extraOreiantation) * Mathf.Deg2Rad;
            Vector2 vec = new Vector2(Mathf.Cos(rads), Mathf.Sin(rads));

            GameObject bulletToSpawn = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 0) * vec), transform.parent);
            sendBulletWithDamageInfo(bulletToSpawn.GetComponent<Bullet>(), vec);
        }
        playShootSound();
    }
}
        


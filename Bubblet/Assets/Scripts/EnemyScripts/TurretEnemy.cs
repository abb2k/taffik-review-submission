using UnityEngine;

public class TurretEnemy : EnemyBase
{
    [Header("Turret")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float recoil;

    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void OnShoot()
    {
        animator.SetTrigger("Attack");

    }

    private void Update()
    {
        if (wasCaught) return;

        Vector3 diff = GameManager.get().Bubble.transform.position - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }

    private void Shoot()
    {
        GameObject bulletToSpawn = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 0) * transform.right), transform.parent);
        sendBulletWithDamageInfo(bulletToSpawn.GetComponent<Bullet>(), transform.right);
        playShootSound();
        rb.AddForce(-transform.right * recoil);
    }
}

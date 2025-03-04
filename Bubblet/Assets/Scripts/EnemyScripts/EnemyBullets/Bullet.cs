using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected DamageInfo info = new DamageInfo();
    [SerializeField] protected float speed;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] Transform KBOrigin;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        LevelManager LManager = GameManager.get().getLevelManager();

        if (LManager)
            LManager.addToCleanup(transform);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ihittable hittable))
            OnHit(hittable);

        if (collision.tag.Equals("border"))
            onBorderCollision();
    }

    protected virtual void onBorderCollision()
    {
        Destroy(gameObject);
    }

    protected virtual void OnHit(Ihittable hit)
    {
        hit.OnHit(info, KBOrigin);
    }

    public void send(Vector2 direction)
    {
        send(direction, speed);
    }

    public virtual void send(Vector2 direction, float speed)
    {
        this.speed = speed;

        rb.linearVelocity = direction * this.speed;
    }

    public void setInfo(DamageInfo info)
    {
        this.info = info;
    }

    public void setKBOrigin(Transform KBOrigin)
    {
        this.KBOrigin = KBOrigin;
    }

    public void disable()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        if (TryGetComponent(out Collider2D col))
        {
            col.enabled = false;
        }
        this.enabled = false;
    }
}

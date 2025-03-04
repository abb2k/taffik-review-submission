using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletCapture : MonoBehaviour
{
    private ICatchable catchAble = null;
    [SerializeField] private float absorbeTime;
    [SerializeField] private AnimationCurve absorbeCurve;
    [SerializeField] private bool canConsume;
    [SerializeField] private float speed;
    [SerializeField] private float slowedSpeed;
    public Rigidbody2D rb;
    [SerializeField] private Ranged Scale;

    private Vector2 movementDir;

    private void Start()
    {
        transform.localScale = Vector2.one * Scale.newRandom;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkForCatch(collision.gameObject);

        checkForConsume(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        checkForCatch(collision.gameObject);

        checkForConsume(collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        checkForConsume(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        checkForConsume(collision.gameObject);
    }

    void checkForCatch(GameObject collision)
    {
        if (collision.TryGetComponent(out ICatchable c))
            captureBullet(c);
    }

    void checkForConsume(GameObject collision)
    {
        if (collision.TryGetComponent(out PlayerReference pRef))
            consume(pRef.Player);
    }

    async void captureBullet(ICatchable catchable)
    {
        if (catchAble != null) return;
        catchAble = catchable;

        catchAble.OnCatch();

        Transform toCatch = catchAble.getCapturedObject();

        toCatch.SetParent(transform, true);

        Vector3 originalPos = toCatch.localPosition;

        rb.AddForce(-movementDir * slowedSpeed);

        canConsume = true;

        for (float t = 0; t < absorbeTime; t += Time.deltaTime)
        {
            if (toCatch == null) break;

            toCatch.localPosition = Vector3.Lerp(originalPos, -Vector3.back, absorbeCurve.Evaluate(t / absorbeTime));

            await Task.Yield();
        }
    }

    void consume(player p)
    {
        if (!canConsume || catchAble == null) return;

        p.AddHealth(catchAble.getHealOnCosume());

        GameManager.get().getLevelManager().addScoreMultiplier(catchAble.getMultiOnConsume());

        FeedbackText text = GameManager.get().CreateFeedbackText(transform.position);
        text.StartTextEffect($"+{catchAble.getMultiOnConsume()} mult");
        text.SetColor(new Color32(188, 124, 252, 255));

        Destroy(gameObject);
    }

    public void send(Vector2 dir)
    {
        rb.AddForce(dir * speed);
        movementDir = dir;
    }
}

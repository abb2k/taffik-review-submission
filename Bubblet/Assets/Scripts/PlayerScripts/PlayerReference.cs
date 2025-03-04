using UnityEngine;

public class PlayerReference : MonoBehaviour, Ihittable
{
    public player Player;

    public void OnHit(DamageInfo info, Transform KBOrigin = null)
    {
        Player.OnDamage(info, KBOrigin);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IMenuInteractable interactable))
            Player.OnMenuInteractable(interactable, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IMenuInteractable interactable))
            Player.OnMenuInteractable(interactable, false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class Item : MonoBehaviour
{
    [BoxGroup("Item Default")]
    public SpriteRenderer sr;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            OnCollect(collision.GetComponent<Player>());
        }
    }

    public virtual void OnCollect(Player player) { }
}

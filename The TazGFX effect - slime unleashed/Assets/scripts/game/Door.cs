using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using NaughtyAttributes;

public class Door : MonoBehaviour, IInteractable
{
    [BoxGroup("Door Settings")]
    [SerializeField] private SceneAsset Scene;
    [BoxGroup("Door Settings")]
    public string DoorName;
    [BoxGroup("Door Settings")]
    [SerializeField] private string LinkedDoorName;
    [BoxGroup("Door Settings")]
    [SerializeField] private Transform exitPos;

    [BoxGroup("Door Settings")]
    private bool playerNear;

    public void goInDoorWithoutSpecific(string Scene, string DoorTo, Player player)
    {
        player.DisableInput = true;
        player.collision = false;
        player.animator.SetBool("isJumping", true);
        StartCoroutine(goInDoor());
        IEnumerator goInDoor()
        {
            GameManager Gm = GameManager.getShared();

            Gm.AllowInput(false);

            bool moved = false;
            while (true)
            {
                if (player.canMove)
                {
                    var pos = player.transform.position;
                    pos.x = Mathf.Lerp(pos.x, transform.position.x, 12 * Time.deltaTime / 4);
                    pos.y = Mathf.Lerp(pos.y, transform.position.y, 12 * Time.deltaTime / 4);
                    player.transform.position = pos;
                    player.sr.color = Color.Lerp(player.sr.color, GameManager.SetColorAlpha(player.sr.color, 0), 12 * Time.deltaTime);

                    moved = true;
                }
                else if (moved)
                {
                    player.sr.color = GameManager.SetColorAlpha(player.sr.color, 0);
                }
                if (moved)
                {
                    var col = Gm.fgBlackScreen.color;
                    col.a = Mathf.Lerp(col.a, 1, 12 * Time.deltaTime / 4);
                    Gm.fgBlackScreen.color = col;
                }
                if (Gm.fgBlackScreen.color.a >= 0.99f)
                {
                    break;
                }
                yield return null;
            }
            var colo = Gm.fgBlackScreen.color;
            colo.a = 1;
            Gm.fgBlackScreen.color = colo;
            player.animator.SetBool("isJumping", false);
            GameManager.getShared().MoveToScene(Scene, DoorTo);
        }
    }

    public void exitDoor(Player player)
    {
        StartCoroutine(goOutofDoor(player));
        IEnumerator goOutofDoor(Player player)
        {
            player.transform.position = transform.position;


            yield return new WaitForSeconds(0.3f);
            var camPos = GameManager.getShared().Camera.transform.position;
            camPos.x = transform.position.x;
            camPos.y = transform.position.y;
            GameManager.getShared().Camera.transform.position = camPos;

            player.animator.SetBool("isJumping", true);
            GameManager Gm = GameManager.getShared();

            yield return null;
            bool moved = false;
            while (true)
            {
                if (player.canMove)
                {
                    var pos = player.transform.position;
                    pos.x = Mathf.Lerp(pos.x, exitPos.position.x, 12 * Time.deltaTime / 4);
                    pos.y = Mathf.Lerp(pos.y, exitPos.position.y, 12 * Time.deltaTime / 4);
                    pos.z = 0;
                    player.transform.position = pos;
                    player.sr.color = Color.Lerp(player.sr.color, GameManager.SetColorAlpha(player.sr.color, 1), 12 * Time.deltaTime);

                    moved = true;
                }
                else if (moved)
                {
                    player.sr.color = GameManager.SetColorAlpha(player.sr.color, 1);
                }
                if (moved)
                {
                    var col = Gm.fgBlackScreen.color;
                    col.a = Mathf.Lerp(col.a, 0, 12 * Time.deltaTime / 2);
                    Gm.fgBlackScreen.color = col;
                }
                if (Gm.fgBlackScreen.color.a <= 0.01f)
                {
                    break;
                }
                yield return null;
            }
            player.DisableInput = false;
            player.collision = true;
            var colo = Gm.fgBlackScreen.color;
            colo.a = 0;
            Gm.fgBlackScreen.color = colo;
            player.animator.SetBool("isJumping", false);
            Gm.AllowInput(true);
        }
    }

    public void onInteract(Player player)
    {
        if (player.DisableInput || player.InJump) return;

        player.DisableInput = true;
        player.collision = false;
        player.animator.SetBool("isJumping", true);
        StartCoroutine(goInDoor());
        IEnumerator goInDoor()
        {
            GameManager Gm = GameManager.getShared();

            Gm.AllowInput(false);

            bool moved = false;
            while (true)
            {
                if (player.canMove)
                {
                    var pos = player.transform.position;
                    pos.x = Mathf.Lerp(pos.x, transform.position.x, 12 * Time.deltaTime / 4);
                    pos.y = Mathf.Lerp(pos.y, transform.position.y, 12 * Time.deltaTime / 4);
                    player.transform.position = pos;
                    player.sr.color = Color.Lerp(player.sr.color, GameManager.SetColorAlpha(player.sr.color, 0), 12 * Time.deltaTime);

                    moved = true;
                }
                else if (moved)
                {
                    player.sr.color = GameManager.SetColorAlpha(player.sr.color, 0);
                }
                if (moved)
                {
                    var col = Gm.fgBlackScreen.color;
                    col.a = Mathf.Lerp(col.a, 1, 12 * Time.deltaTime / 4);
                    Gm.fgBlackScreen.color = col;
                }
                if (Gm.fgBlackScreen.color.a >= 0.99f)
                {
                    break;
                }
                yield return null;
            }
            var colo = Gm.fgBlackScreen.color;
            colo.a = 1;
            Gm.fgBlackScreen.color = colo;
            player.animator.SetBool("isJumping", false);
            GameManager.getShared().MoveToScene(Scene.name, LinkedDoorName);
        }
    }
}

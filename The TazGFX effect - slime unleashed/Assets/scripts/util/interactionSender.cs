using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactionSender : MonoBehaviour
{
    [SerializeField] private Player player;

    List<IInteractable> interactables = new List<IInteractable>();

    [SerializeField] List<GameObject> interactablesObj = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            interactables.Add(interactable);
            interactablesObj.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            if (interactables.Contains(interactable))
            {
                interactables.Remove(interactable);
                interactablesObj.Remove(collision.gameObject);
            }
        }
    }

    private void Update()
    {
        checkInteractInput();
    }

    void checkInteractInput()
    {
        if (!Input.GetKeyDown(KeyCode.E) || interactables.Count == 0 || player == null) return;
        //when E is pressed and interactables are in range:

        interactables[0].onInteract(player);

        //move current interactions to the end of the list
        IInteractable tempInteractable = interactables[0];
        interactables.RemoveAt(0);
        interactables.Add(tempInteractable);
    }
}

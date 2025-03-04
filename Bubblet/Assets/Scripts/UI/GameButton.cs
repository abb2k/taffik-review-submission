using NUnit.Framework;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

[SelectionBase]
public class GameButton : MonoBehaviour, IMenuInteractable
{
    private player myPlayer = null;

    [SerializeField] private ButtonClickedEvent e;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoveredColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Animator ButtonAnim;

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    private Coroutine clickAnimRoutiene = null;

    private void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        if (!image || !text) return;
        image.color = defaultColor;
        text.color = defaultColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerReference p))
            onHover(p.Player, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerReference p))
            onHover(p.Player, false);
    }

    void onHover(player p, bool enter)
    {
        if (!image || !text) return;

        image.color = enter ? hoveredColor : defaultColor;
        text.color = enter ? hoveredColor : defaultColor;

        myPlayer = enter ? p : null;
    }

    IEnumerator clickAnim()
    {
        yield return null;

        clickAnimRoutiene = null;
    }

    public void onInteractEnter(player p)
    {
        //clicked
        if (clickAnimRoutiene != null || !this.enabled) return;
        

        clickAnimRoutiene = StartCoroutine(clickAnim());
        
        e.Invoke();
        if (ButtonAnim == null) return;
        ButtonAnim.Play("ButtonClick");
    }

    public void onInteractExit(player p)
    {

    }
}

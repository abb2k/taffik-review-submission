using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NaughtyAttributes;

public class SlimyButton : MonoBehaviour
{
    [BoxGroup("Button Base")]
    [SerializeField] private Sprite white;
    [BoxGroup("Button Base")]
    [SerializeField] private Sprite red;

    [BoxGroup("Butto Base")]
    [SerializeField] private Transform Arrow;

    [BoxGroup("Button Base")]
    [SerializeField] private Image image;

    [BoxGroup("Event")]
    [SerializeField] private Button.ButtonClickedEvent clickEvent;

    public void OnClick() 
    {
        clickEvent.Invoke();
    }
    public void OnEnter() 
    {
        image.sprite = red;
        var pos = Arrow.position;
        pos.y = transform.position.y;
        Arrow.position = pos;
        Arrow.gameObject.SetActive(true);
    }
    public void OnExit() 
    {
        image.sprite = white;
        Arrow.gameObject.SetActive(false);
    }

    void Start()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerClick = new EventTrigger.Entry();
        pointerClick.eventID = EventTriggerType.PointerClick;
        pointerClick.callback.AddListener((eventData) => { OnClick(); });
        trigger.triggers.Add(pointerClick);

        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((eventData) => { OnEnter(); });
        trigger.triggers.Add(pointerEnter);

        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((eventData) => { OnExit(); });
        trigger.triggers.Add(pointerExit);
    }
}

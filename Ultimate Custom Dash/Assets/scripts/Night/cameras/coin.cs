using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class coin : MonoBehaviour
{
    NightManager nm;

    [SerializeField] GraphicRaycaster raycaster;

    [SerializeField] PointerEventData pointerEventData;

    [SerializeField] EventSystem eventSystem;

    bool collectEnter;

    private void Update()
    {
        if (raycaster == null)
        {
            raycaster = NightManager.inctance.raycaster;
        }

        if (eventSystem == null)
        {
            eventSystem = NightManager.inctance.eventSystem;
        }
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(pointerEventData, results);
        if (results.Count == 0)
        {
            collectEnter = false;
        }
        foreach (RaycastResult result in results)
        {
            if (result.gameObject == gameObject)
            {
                if (!collectEnter)
                {
                    collectEnter = true;
                    OnCollect();
                }
                else
                {
                    collectEnter = false;
                }
            }
        }
    }

    public void OnCollect()
    {
        nm = NightManager.inctance;
        nm.addFazCoins(1);
        nm.CamSys.RemoveCoin(gameObject);
    }
}

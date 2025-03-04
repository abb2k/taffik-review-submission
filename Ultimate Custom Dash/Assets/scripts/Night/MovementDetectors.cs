using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MovementDetectors : MonoBehaviour
{
    public cameraMovement movement;
    enum directions { non, leftSlow, leftFast, rightSlow, rightFast };
    [SerializeField] directions direction;

    [SerializeField] GraphicRaycaster raycaster;

    [SerializeField] PointerEventData pointerEventData;

    [SerializeField] EventSystem eventSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == gameObject)
            {
                switch (direction)
                {
                    case directions.non:
                        movement.setSpeed(0);
                        break;
                    case directions.leftSlow:
                        movement.setSpeed(-8);
                        break;
                    case directions.leftFast:
                        movement.setSpeed(-22);
                        break;
                    case directions.rightSlow:
                        movement.setSpeed(8);
                        break;
                    case directions.rightFast:
                        movement.setSpeed(22);
                        break;
                }
            }
        }
    }
}

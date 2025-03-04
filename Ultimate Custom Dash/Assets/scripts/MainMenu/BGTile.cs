using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGTile : MonoBehaviour
{
    public enum BGTileStates { Red, Blue, Filled, Empty };
    public BGTileStates State;
    [SerializeField]
    float stateStayTime;
    [SerializeField]
    float currentStateStartTime;
    public Image image;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        ChangeTileState();
    }

    // Update is called once per frame
    void Update()
    {
        if (stateStayTime > 0)
        {
            stateStayTime -= Time.deltaTime;
        }
        else
        {
            ChangeTileState();
        }
        if (State == BGTileStates.Filled)
        {
            float opacity = (0.2f / currentStateStartTime * stateStayTime) - 0.2f / (currentStateStartTime - 3);
            if (opacity <= 0)
            {
                opacity = 0;
            }

            image.color = new Color(1, 1, 1, opacity);
        }
        else
        {
            image.color = new Color(1, 1, 1, 0.2f);
        }
    }

    void ChangeTileState()
    {
        int randomize = Random.Range(0, 8);
        int stateSelected = randomize;

        if (randomize == 0 || randomize == 1 || randomize == 7)
        {
            stateSelected = 0;
        }
        if (randomize == 2 || randomize == 3 || randomize == 8)
        {
            stateSelected = 1;
        }
        if (randomize == 4)
        {
            stateSelected = 2;
        }
        if (randomize == 5 || randomize == 6)
        {
            stateSelected = 3;
        }

        anim.SetInteger("state", stateSelected);
        State = (BGTileStates)stateSelected;

        stateStayTime = Random.Range(5.0f , 10.0f);
        currentStateStartTime = stateStayTime;

        switch (State)
        {
            case BGTileStates.Red:
                break;
            case BGTileStates.Blue:
                break;
            case BGTileStates.Filled:
                break;
            case BGTileStates.Empty:
                break;
        }
    }
}

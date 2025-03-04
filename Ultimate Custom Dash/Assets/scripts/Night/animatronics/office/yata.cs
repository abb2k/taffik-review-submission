using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class yata : AnimatronicBase
{
    public GameObject yataS;
    public SpriteRenderer yataRend;

    bool flyToRight;

    bool clickedGreen;

    public float movementSpeed;

    bool oneTimeClickLock;

    bool isMoving;

    public override void AnimatronicGameStart()
    {
        AddCustomValue(new FloatValue(movementSpeed, "movementSpeed"));
    }

    //called when animatronic gets his AILevel
    public override void AnimatronicStart()
    {
        
    }

    //called every frame after the Oppretunity calculations
    public override void AnimatronicUpdate()
    {
        if (NM.NightOngoing)
        {
            if (GameManager.DetectClickedOutside(yataRend.gameObject, false) && !NM.CamsFullyOpened)
            {
                if (!oneTimeClickLock)
                {
                    oneTimeClickLock = true;
                    OnClickedBird();
                }
            }
            else
            {
                oneTimeClickLock = false;
            }

            if (isMoving)
            {
                if (flyToRight)
                {
                    var pos = yataS.transform.localPosition;
                    pos.x = Mathf.MoveTowards(pos.x, 15.4f, movementSpeed * Time.deltaTime);
                    yataS.transform.localPosition = pos;

                    if (pos.x >= 15.4f)
                    {
                        isMoving = false;
                        reahcedEnd();
                    }
                }
                else
                {
                    var pos = yataS.transform.localPosition;
                    pos.x = Mathf.MoveTowards(pos.x, -15.4f, movementSpeed * Time.deltaTime);
                    yataS.transform.localPosition = pos;

                    if (pos.x <= -15.4f)
                    {
                        isMoving = false;
                        reahcedEnd();
                    }
                }
            }
        }
    }

    //called every oppretunity
    public override void OnOppretunity()
    {
        int randomcol = Random.Range(0,2);
        if (randomcol == 0)
        {
            yataRend.color = Color.red;
        }
        else if (randomcol == 1)
        {
            yataRend.color = Color.green;
        }

        int randomdir = Random.Range(0, 2);
        if (randomdir == 0)
        {
            flyToRight = true;
        }
        else if (randomdir == 1)
        {
            flyToRight = false;
        }

        if (flyToRight)
        {
            yataS.transform.position = new Vector3(-15.4f, 0, yataS.transform.position.z);
            yataS.transform.localScale = new Vector3(-0.7f, 0.7f, 0.7f);
        }
        else
        {
            yataS.transform.position = new Vector3(15.4f,0, yataS.transform.position.z);
            yataS.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        isMoving = true;
        GM.soundManager.CreateSoundEffect("birdScream", GM.soundManager.GetSoundFromList("birdScream"));
    }

    //called when deathcoined
    public override void OnDeathcoined()
    {
        base.OnDeathcoined();
        yataS.SetActive(false);
        yataRend.color = Color.red;

        if (flyToRight)
        {
            yataS.transform.localPosition = new Vector3(15.4f, yataS.transform.localPosition.y, yataS.transform.localPosition.z);
        }
        else
        {
            yataS.transform.localPosition = new Vector3(-15.4f, yataS.transform.localPosition.y, yataS.transform.localPosition.z);
        }
    }

    //called when someone kills the player
    public override void OnPlayerDied()
    {
        yataS.SetActive(false);
    }

    void reahcedEnd()
    {
        if (yataRend.color.g == 1)
        {
            //green
            yataS.SetActive(true);
            if (!clickedGreen)
            {
                //kill
                Jumpscare();
            }
        }
        clickedGreen = false;
    }

    public virtual void OnClickedBird()
    {
        if (yataRend.color.r == 1)
        {
            //red
            yataS.SetActive(false);
            Jumpscare();
        }
        else
        {
            //green
            yataS.SetActive(false);
            clickedGreen = true;
            PlayPoof().transform.position = yataRend.transform.position;
        }
    }

    public override void SetCustomValue(FloatValue value)
    {
        if (value.keyName == "movementSpeed")
        {
            movementSpeed = value.value;
        }

        base.SetCustomValue(value);
    }

    public override void OnMirrorSummon(int AIChosen)
    {
        base.OnMirrorSummon(AIChosen);


    }
}

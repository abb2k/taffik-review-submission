using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offcie : MonoBehaviour
{
    public NightManager nm;
    public offcieLayer.offices office;
    [ShowAssetPreview]
    public Sprite offcieLight;
    [ShowAssetPreview]
    public Sprite offcieDark;
    [ShowAssetPreview]
    public Sprite offciePowerOut;
    [ShowAssetPreview]
    public Sprite offcieSL;
    [ShowAssetPreview]
    public Sprite offcieF3TVOff;
    [ShowAssetPreview]
    public Sprite offcieF3TVOn;
    [ShowAssetPreview]
    public Sprite offcieF4;

    public SpriteRenderer officeSprite;
    public GameObject Light;
    public GameObject LightGlow;
    public SpriteRenderer Springtarp;
    public enum officeLightings { flickring, PowerDown }
    public officeLightings lighting;

    public GameObject blueParticals;
    public GameObject orangeParticals;
    public GameObject ventBGFix;
    public Transform brV;

    [ReadOnly][SerializeField] float flickerTimer;
    [ReadOnly][SerializeField] float TVScreenFlicker;
    [ReadOnly][SerializeField] float SpringtrapAppearTimer;
    [ReadOnly][SerializeField] float SpringtrapStayTimer;
    void Start()
    {
        flickerTimer = Random.Range(0.02f, 1);
        TVScreenFlicker = 0.02f;
        SpringtrapAppearTimer = Random.Range(1.0f, 4.0f);
        SpringtrapStayTimer = Random.Range(0.5f, 1.0f);
        Springtarp.enabled = false;
        GameManager GM = GameManager.get();
        if (GM)
        {
            office = (offcieLayer.offices)GM.SaveData.SelectedOffcie;
        }
        

        if (office == offcieLayer.offices.SL)
        {
            officeSprite.sprite = offcieSL;
        }
        else if (office == offcieLayer.offices.Fnaf3)
        {
            officeSprite.sprite = offcieF3TVOff;
        }
        else if (office == offcieLayer.offices.Fnaf4)
        {
            officeSprite.sprite = offcieF4;
        }
        else
        {
            blueParticals.SetActive(true);
            orangeParticals.SetActive(true);
            ventBGFix.SetActive(true);

            brV.position = new Vector3(11.47f, -4, -5);
            brV.localScale = Vector3.one * 0.88f;
        }
    }

    void Update()
    {
        if (office == offcieLayer.offices.Default)
        {
            normalOfficeUpdate();
        }
        else if (office == offcieLayer.offices.SL)
        {

        }
        else if (office == offcieLayer.offices.Fnaf3)
        {
            FNaf3OfficeUpdate();
        }
        else if (office == offcieLayer.offices.Fnaf4)
        {

        }
    }

    void FNaf3OfficeUpdate()
    {
        if (TVScreenFlicker > 0)
        {
            TVScreenFlicker -= Time.deltaTime;
        }
        else
        {
            TVScreenFlicker = Random.Range(0.01f, 0.05f);
            if (officeSprite.sprite == offcieF3TVOff)
            {
                officeSprite.sprite = offcieF3TVOn;
                Springtarp.color = new Color32(95, 95, 95, 255);
            }
            else if (officeSprite.sprite == offcieF3TVOn)
            {
                officeSprite.sprite = offcieF3TVOff;
                Springtarp.color = Color.white;
            }
        }

        if (SpringtrapAppearTimer > 0)
        {
            SpringtrapAppearTimer -= Time.deltaTime;
        }
        else
        {
            Springtarp.enabled = true;

            if (SpringtrapStayTimer > 0)
            {
                SpringtrapStayTimer -= Time.deltaTime;
            }
            else
            {
                Springtarp.enabled = false;
                SpringtrapAppearTimer = Random.Range(1.0f, 8.0f);
                SpringtrapStayTimer = Random.Range(0.5f, 2.0f);
            }
        }
    }

    void normalOfficeUpdate()
    {
        if (nm.NightOngoing)
            switch (lighting)
            {
                case officeLightings.flickring:
                    if (officeSprite.sprite == offciePowerOut)
                    {
                        officeSprite.sprite = offcieDark;
                        ventBGFix.GetComponent<SpriteRenderer>().color = new Color32(159, 159, 159, 255);
                    }
                    if (flickerTimer > 0)
                    {
                        flickerTimer -= Time.deltaTime;

                        if (flickerTimer <= 0)
                        {
                            flickerTimer = Random.Range(0.02f, 1);

                            if (officeSprite.sprite == offcieLight)
                            {
                                officeSprite.sprite = offcieDark;
                                Light.SetActive(false);
                                LightGlow.SetActive(false);
                                ventBGFix.GetComponent<SpriteRenderer>().color = new Color32(159, 159, 159, 255);
                            }
                            else if (officeSprite.sprite == offcieDark)
                            {
                                officeSprite.sprite = offcieLight;
                                Light.SetActive(true);
                                LightGlow.SetActive(true);
                                ventBGFix.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                            }
                        }
                    }
                    break;
                case officeLightings.PowerDown:
                    officeSprite.sprite = offciePowerOut;
                    Light.SetActive(false);
                    LightGlow.SetActive(false);
                    ventBGFix.GetComponent<SpriteRenderer>().color = new Color32(91, 91, 91, 255);
                    break;
            }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FireworkBlast : MonoBehaviour
{
    public Image YellowGlow;
    public CanvasScaler cs;
    GameManager GM;
    public Sprite
        starBlue,
        starGreen,
        starPink,
        starRed,
        starSmall,
        starYellow
    ;
    List<star> stars = new List<star>();
    bool startedRuning;
    float speed = 1400;
    float opaci = 175;

    public void RunBlast(CanvasScaler scaler)
    {
        int randomSound = Random.Range(0, 2);

        GM = GameManager.get();

        cs = scaler;

        speed = 7;

        startedRuning = true;

        switch (randomSound)
        {
            case 0:
                GM.soundManager.CreateSoundEffect("fireworkBlast", GM.soundManager.GetSoundFromList("blast1"));
                break;
            case 1:
                GM.soundManager.CreateSoundEffect("fireworkBlast", GM.soundManager.GetSoundFromList("blast2"));
                break;

            default:
                break;
        }

        int randomGlowColor = Random.Range(0, 3);

        switch (randomGlowColor)
        {
            case 0:
                YellowGlow.color = new Color32((byte)(Color.yellow.r * 255), (byte)(Color.yellow.g * 255), (byte)(Color.yellow.b * 255), 175);
                break;
            case 1:
                YellowGlow.color = new Color32(150, 150, (byte)(Color.blue.b * 255), 175);
                break;
            case 2:
                YellowGlow.color = new Color32(150, (byte)(Color.green.g * 255), 150, 175);
                break;
        }

        int startsAmount = Random.Range(15, 40);

        for (int i = 0; i < startsAmount; i++)
        {
            star s = new star();
            s.dir.x = Random.Range(-1.0f, 1.0f);
            s.dir.y = Random.Range(-1.0f, 1.0f);

            int starSelected = Random.Range(0, 5);
            switch (starSelected)
            {
                case 0:
                    s.Sprite = starBlue;
                    break;

                case 1:
                    s.Sprite = starGreen;
                    break;

                case 2:
                    s.Sprite = starPink;
                    break;

                case 3:
                    s.Sprite = starRed;
                    break;

                case 4:
                    s.Sprite = starSmall;
                    break;

                case 5:
                    s.Sprite = starYellow;
                    break;
            }

            s.myObj = Instantiate(GM.emptyObject, transform);
            s.myObj.transform.localScale = new Vector3(2, 2, 2);
            Image myImage = s.myObj.AddComponent<Image>();
            myImage.sprite = s.Sprite;
            myImage.SetNativeSize();

            stars.Add(s);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (startedRuning)
        {
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].myObj.transform.position = new Vector3(stars[i].myObj.transform.position.x + (stars[i].dir.x * speed * Time.deltaTime), stars[i].myObj.transform.position.y + (stars[i].dir.y * speed * Time.deltaTime), 0);
                stars[i].myObj.GetComponent<Image>().color = new Color32(255, 255, 255, (byte)opaci);
                stars[i].myObj.transform.position -= new Vector3(0, 0.25f * Time.deltaTime, 0);
            }
            if (speed > 0)
            {
                
                speed -= Time.deltaTime * 7.225f;

                if (speed <= 0)
                {
                    speed = 0;
                }
            }
            if (speed <= 0)
            {
                speed = 0;

                if (opaci > 0)
                {
                    opaci -= Time.deltaTime * 400;

                    if (opaci <= 0)
                    {
                        opaci = 0;
                    }
                }
                if (opaci <= 0)
                {
                    opaci = 0;
                    Destroy(gameObject);
                }

            }
            var col = YellowGlow.color;
            if (YellowGlow.color.a > 0)
            {

                col.a -= (Time.deltaTime * 700) / 255;

                if (col.a <= 0)
                {
                    col.a = 0;
                }
            }
            if (col.a <= 0)
            {
                col.a = 0;
            }
            YellowGlow.color = col;
        }
    }
}

public class star
{
    public Sprite Sprite;
    public Vector2 dir;
    public GameObject myObj;
}
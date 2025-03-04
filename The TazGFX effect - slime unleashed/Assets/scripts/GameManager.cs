using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public GameObject Camera;

    public GameObject EmptyObj;
    public GameObject FloatingTextObj;
    public GameObject InputBlocker;

    public Image fgBlackScreen;

    public GameObject DontDest;

    private static GameManager inctance;

    private void Awake()
    {
        if (inctance == null)
        {
            inctance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (DontDest)
        {
            Destroy(DontDest);
        }
        else
        {
            GameObject destrythispls = GameObject.Find("----DontDestroy----");
            if (destrythispls)
            {
                Destroy(destrythispls);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SceneManager.LoadScene("sampleLevel");
        }
    }

    public void AllowInput(bool b)
    {
        InputBlocker.SetActive(!b);
    }
    public static GameManager getShared()
    {
        return inctance;
    }

    public static Color SetColorAlpha(Color color, float Alpha)
    {
        return new Color(color.r, color.g, color.b, Alpha);
    }

    public void MoveToScene(string sceneName, string doorTo)
    {
        StartCoroutine(moveScene(sceneName, doorTo));
        IEnumerator moveScene(string sceneName, string doorTo)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Camera.GetComponent<cameraMovement>().StopInfinitAnims();

            Door[] doors = Door.FindObjectsOfType<Door>();

            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i].DoorName == doorTo)
                {
                    doors[i].exitDoor(player);
                }
            }
        }
    }
}

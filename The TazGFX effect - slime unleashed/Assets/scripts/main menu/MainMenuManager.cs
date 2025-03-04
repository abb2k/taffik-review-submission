using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static cameraMovement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Door door;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.getShared().Camera.GetComponent<cameraMovement>().cameraPositionAnimation(new CameraAnimationPosition[]
{
            new CameraAnimationPosition
            {
                position = new Vector2(-3.3f, -1.77f),
                speedType = CameraAnimationPosition.SpeedType.Custom,
                zoomSpeedType = CameraAnimationPosition.ZoomSpeedType.Custom,
                CustomSpeed = 1000,
                ZoomSpeed = 1000,
                ZoomAmount = 5.2f,
                Infinit = true
            }
        });
    }

    public void OnContinue()
    {
        door.goInDoorWithoutSpecific("Level1", "level1Entrence", GameManager.getShared().player);
    }

    public async void OnLeaveGame()
    {
        GameManager GM = GameManager.getShared();
        GM.AllowInput(false);
        while (true)
        {
            GM.fgBlackScreen.color = Color.Lerp(GM.fgBlackScreen.color, Color.black, 2f * Time.deltaTime);
            if (GM.fgBlackScreen.color.a >= 0.999f)
            {
                break;
            }
            await Task.Yield();
        }
        var colo = GM.fgBlackScreen.color;
        colo.a = 1;
        GM.fgBlackScreen.color = colo;
        await Task.Delay(1000);
        Application.Quit();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public player Bubble;
    public Camera mainCamera;
    private MenuManager MManager;
    private LevelManager LevelManager;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(Bubble.gameObject);
            DontDestroyOnLoad(mainCamera.gameObject);
        }
        else
        {
            Destroy(gameObject);
            Destroy(Bubble.gameObject);
            Destroy(mainCamera.gameObject);
        }

        SceneManager.activeSceneChanged += onSceneChanged;
    }

    public static GameManager get() { return instance; }

    public GameObject turrentPrefab;
    public GameObject treePrefab;
    public GameObject cactusPrefab;
    public GameObject wavePrefab;
    public GameObject emptyObject;
    public GameObject feedbackTextPrefab;

    public GameObject getPrefabBasedOnEnum(Enemies enemie)
    {
        switch (enemie)
        {
            case Enemies.Tree:
                return treePrefab;
            case Enemies.Turret:
                return turrentPrefab;
            case Enemies.Cactus:
                return cactusPrefab;
            default:
                return null;
        }
    }

    public static bool GetInputDown(Keybind bind)
    {
        return bind.key == KeyCode.None ? Input.GetMouseButtonDown(bind.mouseKeyID) : Input.GetKeyDown(bind.key);
    }
    public static bool GetInput(Keybind bind)
    {
        return bind.key == KeyCode.None ? Input.GetMouseButton(bind.mouseKeyID) : Input.GetKey(bind.key);
    }
    public static bool GetInputUp(Keybind bind)
    {
        return bind.key == KeyCode.None ? Input.GetMouseButtonUp(bind.mouseKeyID) : Input.GetKeyUp(bind.key);
    }

    [Space]

    public GameValues defaultValues;

    void onSceneChanged(Scene curr, Scene next)
    {
        MManager = FindFirstObjectByType<MenuManager>();
        LevelManager = FindFirstObjectByType<LevelManager>();
    }

    public MenuManager getMenuManager() {  return MManager; }
    public LevelManager getLevelManager() {  return LevelManager; }

    public void playCameraShake()
    {
        mainCamera.GetComponent<Animator>().Play("CameraShake");
    }

    public FeedbackText CreateFeedbackText(Vector3 position, Transform parent = null)
    {
        GameObject spawnedText = null;
        if (parent)
            spawnedText = Instantiate(feedbackTextPrefab, position, Quaternion.identity, parent);
        else
            spawnedText = Instantiate(feedbackTextPrefab, position, Quaternion.identity);

        return spawnedText.GetComponent<FeedbackText>();
    }
}

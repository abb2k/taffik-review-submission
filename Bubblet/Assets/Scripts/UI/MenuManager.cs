using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameButton playButton;
    [SerializeField] GameButton optionsButton;
    [SerializeField] GameButton quitButton;
    [SerializeField] Animator mainMenu;
    [SerializeField] AudioClip playS;
    [SerializeField] Animator stageComplete;

    [SerializeField] Animator scoreMenu;
    [SerializeField] Animator TutPages;
    private bool inTut = false;
    private int tutPage = 1;
    public void OnPlay()
    {
        playButton.enabled = false;
        optionsButton.enabled = false;
        quitButton.enabled = false;

        mainMenu.SetBool("exit", true);

        GameManager.get().Bubble.isInUI = false;
        GameManager.get().Bubble.isInMainMenu = false;

        LevelManager LManager = GameManager.get().getLevelManager();

        if (!LManager)
        {
            //failed to start game
            return;
        }

        LManager.ResetStats();

        LManager.startNewLevel();

        SoundManager.getSoundManager().CreateSoundEffect("play", playS);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
    public void OnHelp()
    {
        mainMenu.Play("TutorialAnimation");
        inTut = true;
    }
    public void onNextPage()
    {
        switch (tutPage)
        {
            case 1:
                TutPages.Play("1 into 2");
                tutPage = 2;
                break;
            case 2:
                TutPages.Play("2 into 3");
                tutPage = 3;
                break;
            case 3:
                TutPages.Play("3 into 1");
                tutPage = 1;
                break;
        }

    }
    private void Update()
    {
        if (inTut && Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenu.Play("TutorialAnimationExit");
            inTut = false;
        }
    }


    public void onOptions()
    {

    }

    public void pullScoreMenu()
    {
        scoreMenu.Play("SCoreMenuIn");

        scoreMenu.GetComponent<ScoreMenu>().startCounting();
    }

    public void pullBackScoreMenu()
    {
        scoreMenu.Play("ScoreMenuOut");
    }

    public void playStageCompleteAnim()
    {
        stageComplete.Play("StageComplete");
    }
}

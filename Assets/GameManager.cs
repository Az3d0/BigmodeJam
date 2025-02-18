using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public LevelManager levelManager;
    public int winXpAmount = 5;
    public int lossXpAmount = 2;
    [SerializeField]
    private int xpRequiredToGoNextLevel;
    [SerializeField]
    public int currentXp;
    public int startXp = 5;
    public GameObject gameOver;
    public GameObject winPopup;

    public int XPRequiredToGoNextLevel => xpRequiredToGoNextLevel;

    public event Action<int> OnXPUpdated;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();

        ResetVariables(levelManager.levelList.levels[0]);

        //Subscribe to Minigame end
        //Minigame.OnGameEnded += UpdateXp;
        LevelManager.OnLevelLoaded += ResetVariables;


#if UNITY_EDITOR
        //if playtesting from  a specific level, manually update game manager variables
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            Debug.Log("GameManager: Playing from specific level!");
            ResetVariables(levelManager.levelList.levels.Find(level => level.sceneName == SceneManager.GetActiveScene().name));
        }
#endif
        //For level 1, start with a bit of points so you dont die
        //if (levelManager.currentLevelName == "Level1")
        //{
        //    UpdateXp(true);
        //}
    }

    public void UpdateXp(bool win)
    {

        if (win)
        {
            //Set upper threshold for xp to max required
            currentXp += winXpAmount;
            if (currentXp > xpRequiredToGoNextLevel)
            {
                currentXp = xpRequiredToGoNextLevel;
            }
        }
        else
        {
            currentXp -= lossXpAmount;
            //Set bottom threshold for xp to 0
            if (currentXp < 0)
            {
                currentXp = 0; 
            }
        }
        Debug.Log(currentXp);
        OnXPUpdated?.Invoke(currentXp);
        CheckIfEnoughXp();
        CheckIfZeroXp();
    }

    private void CheckIfEnoughXp()
    {
        if (currentXp >= xpRequiredToGoNextLevel)
        {
            //Trigger some kind of promotion transition/cutscene here
            Debug.Log("You received a PROMOTION!! Go to next level");
            if (!levelManager.LoadNextLevel())
            {
                //Last level - trigger win screen
                winPopup.GetComponent<Tween_Scale>().TriggerScale();
                winPopup.GetComponent<GameOver>().gameOverBackground.SetActive(true);
                GameObject.Find("Player").GetComponent<PlayerControls>().DisablePlayerMovement(true, true);
            }
        }
    }

    private void CheckIfZeroXp()
    {
        if (currentXp == 0)
        {
            
            Debug.Log("You received a DEMOTION!! Go to previous level");
            if (!levelManager.LoadPreviousLevel())
            {
                //Trigger game over screen
                if (gameOver != null)
                {
                    gameOver.GetComponent<Tween_Scale>().TriggerScale();
                    gameOver.GetComponent<GameOver>().gameOverBackground.SetActive(true);
                    GameObject.Find("Player").GetComponent<PlayerControls>().DisablePlayerMovement(true, true);
                    
                }
                else
                {
                    Debug.Log("Assign game over screen in Game Manager!!");
                }
            }
        }
    }

    private void ResetVariables(Level currentLevel)
    {
        //Debug.Log(currentLevel.sceneName + " loaded, new threshold = " + currentLevel.xpThreshold);
        currentXp = startXp;
        xpRequiredToGoNextLevel = currentLevel.xpThreshold;
    }
}

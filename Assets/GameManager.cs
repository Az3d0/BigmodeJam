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
    private int currentXp;

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
        levelManager =  FindFirstObjectByType<LevelManager>();

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
        if (levelManager.currentLevelName == "Level1")
        {
            UpdateXp(true);
        }
    }

    public void UpdateXp(bool win)
    {

        if (win)
        {
            currentXp += winXpAmount; 

        } else
        {
            currentXp -= lossXpAmount;
        }
        Debug.Log(currentXp);
        OnXPUpdated?.Invoke(currentXp);
        CheckIfEnoughXp();
    }

    private void CheckIfEnoughXp()
    {
        if (currentXp >= xpRequiredToGoNextLevel)
        {
            //Trigger some kind of promotion transition/cutscene here
            Debug.Log("You received a PROMOTION!! Go to next level");
            levelManager.LoadNextLevel();
        }
    }

    private void ResetVariables(Level currentLevel)
    {
        //Debug.Log(currentLevel.sceneName + " loaded, new threshold = " + currentLevel.xpThreshold);
        currentXp = 0;
        xpRequiredToGoNextLevel = currentLevel.xpThreshold;
    }
}

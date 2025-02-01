using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public LevelManager levelManager;
    public int winXpAmount = 5;
    public int lossXpAmount = 2;
    [SerializeField]
    private int xpRequiredToGoNextLevel;
    [SerializeField]
    private int currentXp;

    private void OnEnable()
    {
        
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

    }

    private void UpdateXp(bool win)
    {
        if (win)
        {
            currentXp += winXpAmount; 

        } else
        {
            currentXp += lossXpAmount;
        }
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
